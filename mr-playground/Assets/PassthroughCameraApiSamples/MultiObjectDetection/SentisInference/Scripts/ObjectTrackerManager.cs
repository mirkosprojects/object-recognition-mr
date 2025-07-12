using UnityEngine;
using System.Collections.Generic;
using Meta.XR.Samples;
using Unity.Sentis;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PassthroughCameraSamples.MultiObjectDetection
{

    public class ObjectTrackerManager : MonoBehaviour
    {
        [Header("Tracking Settings")]
        public float PositionThreshold = 0.3f;
        public int MinFramesToConfirm = 3;
        public int MaxFramesMissing = 60;

        private List<TrackedObject> trackedObjects = new();

        [Header("Marker Settings")]
        public GameObject markerPrefab;
        public Transform markerParent;

        [Header("Placement configureation")]
        [SerializeField] private EnvironmentRayCastSampleManager m_environmentRaycast;
        [SerializeField] private WebCamTextureManager m_webCamTextureManager;
        private PassthroughCameraEye CameraEye => m_webCamTextureManager.Eye;
        private Vector2Int camRes;

        private string[] m_labels;

        private void Start()
        {
            //Get the camera intrinsics
            var intrinsics = PassthroughCameraUtils.GetCameraIntrinsics(CameraEye);
            camRes = intrinsics.Resolution;
        }

        public void SetLabels(TextAsset labelsAsset)
        {
            //Parse neural net m_labels
            m_labels = labelsAsset.text.Split('\n');
        }

        private Vector3? GetWorldPos(float perX, float perY)
        {
            // Get the 3D marker world position using Depth Raycast
            var centerPixel = new Vector2Int(Mathf.RoundToInt(perX * camRes.x), Mathf.RoundToInt((1.0f - perY) * camRes.y));
            var ray = PassthroughCameraUtils.ScreenPointToRayInWorld(CameraEye, centerPixel);
            return m_environmentRaycast.PlaceGameObjectByScreenPos(ray);
        }

        private List<Detections> GetDetections(Tensor<float> output, Tensor<int> labelIDs, float imageWidth, float imageHeight)
        {
            var detectionsFound = output.shape[0];
            var maxDetections = Mathf.Min(detectionsFound, 200);

            //Get a list of bounding boxes
            List<Detections> Detections = new(maxDetections);
            for (var n = 0; n < maxDetections; n++)
            {
                var worldPos = GetWorldPos(output[n, 0] / imageWidth, output[n, 1] / imageHeight);

                // Create a new bounding box
                var Detection = new Detections
                {
                    ClassName = m_labels[labelIDs[n]].Replace(" ", "_"),
                    WorldPos = worldPos,
                };

                // Add to the list of boxes
                Detections.Add(Detection);
            }
            return Detections;
        }

        public void UpdateTrackedObjects(Tensor<float> output, Tensor<int> labelIDs, float imageWidth, float imageHeight)
        {
            var detections = GetDetections(output, labelIDs, imageWidth, imageHeight);
            // Keep track of which detections matched existing objects
            HashSet<int> matchedDetections = new();

            // Match new detections to existing tracked objects
            foreach (var tracked in trackedObjects)
            {
                bool matched = false;

                for (int i = 0; i < detections.Count; i++)
                {
                    var detection = detections[i];
                    if (matchedDetections.Contains(i)) continue;
                    if (!detection.WorldPos.HasValue) continue;

                    if (IsMatching(tracked, detection))
                    {
                        tracked.Update(detection.WorldPos.Value);
                        matchedDetections.Add(i);
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    tracked.Miss();
                }
            }

            // Add new tracked objects for unmatched detections
            for (int i = 0; i < detections.Count; i++)
            {
                if (matchedDetections.Contains(i)) continue;

                var detection = detections[i];
                if (!detection.WorldPos.HasValue) continue;

                var newObj = new TrackedObject(detection.ClassName, detection.WorldPos.Value, markerPrefab, markerParent);
                trackedObjects.Add(newObj);
            }

            // Remove objects that haven't been seen for a while
            trackedObjects.RemoveAll(obj =>
            {
                bool toRemove = (obj.FramesSeen < MinFramesToConfirm && obj.FramesSinceLastSeen > 5) ||
                                obj.FramesSinceLastSeen > MaxFramesMissing;

                if (toRemove)
                    obj.DestroyMarker();

                return toRemove;
            });
        }

        private bool IsMatching(TrackedObject tracked, Detections detected)
        {
            if (tracked.ClassName != detected.ClassName) return false;
            float dist = Vector3.Distance(tracked.WorldPos, detected.WorldPos.Value);
            return dist < PositionThreshold;
        }

        public List<TrackedObject> GetTrackedObjects()
        {
            return trackedObjects;
        }
    }   
    public struct Detections
    {
        public Vector3? WorldPos;
        public string ClassName;
    }
}