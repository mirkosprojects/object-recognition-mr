using UnityEngine;
using System.Collections.Generic;

namespace PassthroughCameraSamples.MultiObjectDetection
{
    public class ObjectTrackerManager : MonoBehaviour
    {
        public float PositionThreshold = 0.3f;
        public int MinFramesToConfirm = 3;
        public int MaxFramesMissing = 60;

        private List<TrackedObject> trackedObjects = new();

        public void UpdateTrackedObjects(List<BoundingBox> detections)
        {
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

                var newObj = new TrackedObject(detection.ClassName, detection.WorldPos.Value);
                trackedObjects.Add(newObj);
            }

            // Remove objects that haven't been seen for a while
            trackedObjects.RemoveAll(obj =>
                (obj.FramesSeen < MinFramesToConfirm && obj.FramesSinceLastSeen > 5) ||
                obj.FramesSinceLastSeen > MaxFramesMissing);
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