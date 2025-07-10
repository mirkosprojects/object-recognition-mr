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

        [Header("UI display references")]
        [SerializeField] private RawImage m_displayImage;

        private string[] m_labels;

        #region BoundingBoxes functions
        public void SetLabels(TextAsset labelsAsset)
        {
            //Parse neural net m_labels
            m_labels = labelsAsset.text.Split('\n');
        }

        public void SetDetectionCapture(Texture image)
        {
            m_displayImage.texture = image;
        }

        private List<BoundingBox> GetBoxes(Tensor<float> output, Tensor<int> labelIDs, float imageWidth, float imageHeight)
        {
            var displayWidth = m_displayImage.rectTransform.rect.width;
            var displayHeight = m_displayImage.rectTransform.rect.height;

            var scaleX = displayWidth / imageWidth;
            var scaleY = displayHeight / imageHeight;

            var halfWidth = displayWidth / 2;
            var halfHeight = displayHeight / 2;

            var boxesFound = output.shape[0];
            var maxBoxes = Mathf.Min(boxesFound, 200);

            //Get the camera intrinsics
            var intrinsics = PassthroughCameraUtils.GetCameraIntrinsics(CameraEye);
            var camRes = intrinsics.Resolution;

            //Get a list of bounding boxes
            List<BoundingBox> Boxes = new(maxBoxes);
            for (var n = 0; n < maxBoxes; n++)
            {
                // Get bounding box center coordinates
                var centerX = output[n, 0] * scaleX - halfWidth;
                var centerY = output[n, 1] * scaleY - halfHeight;
                var perX = (centerX + halfWidth) / displayWidth;
                var perY = (centerY + halfHeight) / displayHeight;

                // Get object class name
                var classname = m_labels[labelIDs[n]].Replace(" ", "_");

                // Get the 3D marker world position using Depth Raycast
                var centerPixel = new Vector2Int(Mathf.RoundToInt(perX * camRes.x), Mathf.RoundToInt((1.0f - perY) * camRes.y));
                var ray = PassthroughCameraUtils.ScreenPointToRayInWorld(CameraEye, centerPixel);
                var worldPos = m_environmentRaycast.PlaceGameObjectByScreenPos(ray);


                // Debug Logging
                if (worldPos == null)
                {
                    Debug.LogWarning($"Raycast failed at pixel {centerPixel}");
                }
                // Create a new bounding box
                var box = new BoundingBox
                {
                    CenterX = centerX,
                    CenterY = centerY,
                    ClassName = classname,
                    Width = output[n, 2] * scaleX,
                    Height = output[n, 3] * scaleY,
                    Label = $"Id: {n} Class: {classname} Center (px): {(int)centerX},{(int)centerY} Center (%): {perX:0.00},{perY:0.00}, WorldPos: {worldPos}",
                    WorldPos = worldPos,
                };

                // Add to the list of boxes
                Boxes.Add(box);
            }
            return Boxes;
        }
        #endregion

        public void UpdateTrackedObjects(Tensor<float> output, Tensor<int> labelIDs, float imageWidth, float imageHeight)
        {
            var detections = GetBoxes(output, labelIDs, imageWidth, imageHeight);
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

        private bool IsMatching(TrackedObject tracked, BoundingBox detected)
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
}