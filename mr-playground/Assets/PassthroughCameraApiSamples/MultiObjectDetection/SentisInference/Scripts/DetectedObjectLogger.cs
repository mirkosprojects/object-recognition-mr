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
            // Draw line from camera
            foreach (var obj in tracker.GetTrackedObjects())
            {
                Debug.DrawLine(Camera.main.transform.position, obj.WorldPos, Color.green);
                Debug.Log($"Tracked Object {obj.ClassName} at: {obj.WorldPos}, Frames Seen: {obj.FramesSeen}, Frames Since Last Seen: {obj.FramesSinceLastSeen}");
            }
        }
    }

}


