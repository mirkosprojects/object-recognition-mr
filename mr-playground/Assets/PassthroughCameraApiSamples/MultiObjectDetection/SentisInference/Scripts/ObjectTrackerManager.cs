using UnityEngine;
using System.Collections.Generic;
using Unity.Sentis;
using System.Linq;

namespace PassthroughCameraSamples.MultiObjectDetection
{

    public class ObjectTrackerManager : MonoBehaviour
    {
        [Header("Tracking Settings")]
        [SerializeField] private float PositionThreshold = 0.3f;
        [SerializeField] private float mergeThreshold = 0.2f;
        [SerializeField] private int MinFramesToConfirm = 3;
        [SerializeField] private int MaxFramesMissing = 60;
        

        private List<TrackedObject> trackedObjects = new();

        [Header("Marker Settings")]
        [SerializeField] private GameObject markerPrefab;
        [SerializeField] private Transform markerParent;

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

        private List<Detection> GetDetections(Tensor<float> output, Tensor<int> labelIDs, float imageWidth, float imageHeight)
        {
            var detectionsFound = output.shape[0];
            var maxDetections = Mathf.Min(detectionsFound, 200);

            //Get a list of bounding boxes
            List<Detection> Detections = new(maxDetections);
            for (var n = 0; n < maxDetections; n++)
            {
                var worldPos = GetWorldPos(output[n, 0] / imageWidth, output[n, 1] / imageHeight);

                // Create a new bounding box
                var Detection = new Detection
                {
                    ClassName = m_labels[labelIDs[n]].Replace(" ", "_"),
                    WorldPos = worldPos,
                };

                // Add to the list of boxes
                Detections.Add(Detection);
            }
            return Detections;
        }

        private void AddNewTrackedObject(Detection detection)
        {
            var newTracked = new TrackedObject(detection.ClassName, detection.WorldPos.Value, markerPrefab, markerParent);
            trackedObjects.Add(newTracked);
        }

        private HashSet<int> GetDuplicateIDs()
        {
            HashSet<int> indicesToRemove = new HashSet<int>();
            for (int i = 0; i < trackedObjects.Count; i++)
            {
                if (indicesToRemove.Contains(i)) continue;

                for (int j = i + 1; j < trackedObjects.Count; j++)
                {
                    var primary = trackedObjects[i];
                    var secondary = trackedObjects[j];

                    if (indicesToRemove.Contains(j)) continue;
                    if (primary.ClassName != secondary.ClassName) continue;

                    if (Vector3.Distance(primary.WorldPos, secondary.WorldPos) < mergeThreshold)
                    {
                        if (primary.FramesSeen >= secondary.FramesSeen)
                        {
                            indicesToRemove.Add(j);
                        }
                        else
                        {
                            indicesToRemove.Add(i);
                        }
                    }
                }
            }
            return indicesToRemove;
        }

        public void UpdateTrackedObjects(Tensor<float> output, Tensor<int> labelIDs, float imageWidth, float imageHeight)
        {
            var detections = GetDetections(output, labelIDs, imageWidth, imageHeight);

            HashSet<TrackedObject> matchedTrackedObjects = new HashSet<TrackedObject>();
            HashSet<Detection> matchedDetections = new HashSet<Detection>();

            // Match existing tracked objects with current detections
            foreach (var tracked in trackedObjects)
            {
                foreach (var detection in detections)
                {
                    if (IsMatching(tracked, detection))
                    {
                        tracked.Update(detection.WorldPos.Value);
                        matchedTrackedObjects.Add(tracked);
                        matchedDetections.Add(detection);
                    }
                }
            }

            // Handle missed tracked objects
            foreach (var tracked in trackedObjects)
            {
                if (!matchedTrackedObjects.Contains(tracked))
                {
                    tracked.Miss();
                }
            }

            // Handle new detections
            foreach (var detection in detections)
            {
                if (!matchedDetections.Contains(detection))
                {
                    AddNewTrackedObject(detection);
                }
            }

            // Remove objects that haven't been seen for a while
            trackedObjects.RemoveAll(obj =>
            {
                bool toRemove = obj.FramesSinceLastSeen > MaxFramesMissing;

                if (toRemove)
                {
                    obj.DestroyMarker();
                } 

                return toRemove;
            });

            // Remove duplicates from trackedObjects
            var indicesToRemove = GetDuplicateIDs();
            foreach (int index in indicesToRemove.OrderByDescending(i => i))
            {
                trackedObjects[index].DestroyMarker();
                trackedObjects.RemoveAt(index);
            }
        }

        private bool IsMatching(TrackedObject tracked, Detection detected)
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
    public struct Detection
    {
        public Vector3? WorldPos;
        public string ClassName;
    }
}