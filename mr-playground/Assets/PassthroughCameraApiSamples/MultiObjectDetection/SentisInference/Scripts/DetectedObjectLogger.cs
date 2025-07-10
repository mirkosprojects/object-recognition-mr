using UnityEngine;
using System.Collections.Generic;

namespace PassthroughCameraSamples.MultiObjectDetection
{
    public class DetectedObjectLogger : MonoBehaviour
    {
        [SerializeField] private ObjectTrackerManager tracker;

        // Update is called once per frame
        void Update()
        {
            // Debug Logging
            // foreach (var box in BoxDrawn)
            // {
            //     Debug.Log($"Detcted Object {box.ClassName} at: {box.WorldPos}");
            // }

            // tracker.UpdateTrackedObjects(BoxDrawn);

            // Draw line from camera
            foreach (var obj in tracker.GetTrackedObjects())
            {
                Debug.DrawLine(Camera.main.transform.position, obj.WorldPos, Color.green);
                Debug.Log($"Tracked Object {obj.ClassName} at: {obj.WorldPos}, Frames Seen: {obj.FramesSeen}, Frames Since Last Seen: {obj.FramesSinceLastSeen}");
                // Optionally show floating labels or 3D icons
            }
        }
    }

}


