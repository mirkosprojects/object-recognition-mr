using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PassthroughCameraSamples.MultiObjectDetection
{
    public class TrackedObject
    {
        public string ClassName { get; private set; }
        public Vector3 WorldPos { get; private set; }
        public int FramesSinceLastSeen { get; private set; }
        public int FramesSeen { get; private set; }
        public bool IsInitialized { get; private set; } = false;
        private int MinDetectionsToConfirm;
        private Queue<Vector3> recentPositions = new Queue<Vector3>();
        private int AverageFilterSize;
        public GameObject Marker;
        private DetectionSpawnMarkerAnim MarkerAnim;
        public GameObject MarkerPrefab;
        public Transform MarkerParent;

        public TrackedObject(string className, Vector3 worldPos, GameObject markerPrefab, Transform parent, int minDetectionsToConfirm, int averageFilterSize)
        {
            ClassName = className;
            WorldPos = worldPos;
            MarkerPrefab = markerPrefab;
            MarkerParent = parent;
            MinDetectionsToConfirm = minDetectionsToConfirm;
            AverageFilterSize = averageFilterSize;
            FramesSinceLastSeen = 0;
            FramesSeen = 1;
        }

        public void UpdatePosition(Vector3 newPos)
        {
            // Add the new position to the history
            recentPositions.Enqueue(newPos);
            if (recentPositions.Count > AverageFilterSize)
            {
                recentPositions.Dequeue();
            }
                
            // Compute the average of recent positions
            Vector3 averagePos = Vector3.zero;
            foreach (var pos in recentPositions)
            {
                averagePos += pos;
            }
            averagePos /= recentPositions.Count;

            // Smooth update using the averaged position
            WorldPos = Vector3.Lerp(WorldPos, averagePos, 0.5f);

            FramesSinceLastSeen = 0;
            FramesSeen++;

            if (!IsInitialized && FramesSeen >= MinDetectionsToConfirm)
            {
                IsInitialized = true;
                InitializeMarker();
            }
            UpdateMarker();
        }

        public void UpdateFrame()
        {
            FramesSinceLastSeen++;
        }
        
        private void InitializeMarker()
        {
            Marker = GameObject.Instantiate(MarkerPrefab, WorldPos, Quaternion.identity, MarkerParent);
            MarkerAnim = Marker.GetComponent<DetectionSpawnMarkerAnim>();
            if (MarkerAnim != null)
            {
                MarkerAnim.SetYoloClassName(ClassName);
            }
            Marker.SetActive(true);
        }

        public void DestroyMarker()
        {
            if (Marker != null)
            {
                GameObject.Destroy(Marker);
            }
            Marker = null;
        }

        private void UpdateMarker()
        {
            if (Marker != null)
            {
                Marker.transform.position = WorldPos;
            }
        }
    }
}
