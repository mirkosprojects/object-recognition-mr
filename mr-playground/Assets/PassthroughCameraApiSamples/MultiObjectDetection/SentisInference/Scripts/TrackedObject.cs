using UnityEngine;

namespace PassthroughCameraSamples.MultiObjectDetection
{
    public class TrackedObject
    {
        public string ClassName;
        public Vector3 WorldPos;
        public int FramesSinceLastSeen;
        public int FramesSeen;
        public GameObject Marker;
        private DetectionSpawnMarkerAnim MarkerAnim;

        public TrackedObject(string className, Vector3 worldPos, GameObject markerPrefab, Transform parent)
        {
            ClassName = className;
            WorldPos = worldPos;
            FramesSinceLastSeen = 0;
            FramesSeen = 1;

            Marker = GameObject.Instantiate(markerPrefab, worldPos, Quaternion.identity, parent);
            MarkerAnim = Marker.GetComponent<DetectionSpawnMarkerAnim>();

            Marker.SetActive(true);
            UpdateMarker();
        }

        public void Update(Vector3 newPos)
        {
            WorldPos = Vector3.Lerp(WorldPos, newPos, 0.5f); // smooth update
            FramesSinceLastSeen = 0;
            FramesSeen++;
            UpdateMarker();
        }

        public void Miss()
        {
            FramesSinceLastSeen++;
        }

        public void DestroyMarker()
        {
            if (Marker != null)
                GameObject.Destroy(Marker);
                Marker = null;
        }

        private void UpdateMarker()
        {
            if (Marker != null)
            {
                Marker.transform.position = WorldPos;
                // Marker.name = $"Tracked {ClassName}";
            }

            if (MarkerAnim != null)
            {
                MarkerAnim.SetYoloClassName(ClassName);
            }
        }
    }
}
