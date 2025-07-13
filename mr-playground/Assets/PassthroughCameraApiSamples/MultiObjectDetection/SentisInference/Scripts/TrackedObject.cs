using UnityEngine;

namespace PassthroughCameraSamples.MultiObjectDetection
{
    public class TrackedObject
    {
        public string ClassName;
        public Vector3 WorldPos;
        public int FramesSinceLastSeen;
        public int FramesSeen;
        public int MinFramesToConfirm = 3;
        public GameObject Marker;
        private DetectionSpawnMarkerAnim MarkerAnim;
        public GameObject MarkerPrefab;
        public Transform MarkerParent;

        public TrackedObject(string className, Vector3 worldPos, GameObject markerPrefab, Transform parent)
        {
            ClassName = className;
            WorldPos = worldPos;
            MarkerPrefab = markerPrefab;
            MarkerParent = parent;
            FramesSinceLastSeen = 0;
            FramesSeen = 1;
            UpdateMarker();
        }

        public void Update(Vector3 newPos)
        {
            WorldPos = Vector3.Lerp(WorldPos, newPos, 0.5f); // smooth update
            FramesSinceLastSeen = 0;
            FramesSeen++;

            if (Marker == null && FramesSeen >= MinFramesToConfirm)
            {
                InitializeMarker();
            }
            UpdateMarker();
        }

        public void Miss()
        {
            FramesSinceLastSeen++;
        }
        
        private void InitializeMarker()
        {
            Marker = GameObject.Instantiate(MarkerPrefab, WorldPos, Quaternion.identity, MarkerParent);
            MarkerAnim = Marker.GetComponent<DetectionSpawnMarkerAnim>();
            Marker.SetActive(true);
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
            }

            if (MarkerAnim != null)
            {
                MarkerAnim.SetYoloClassName(ClassName);
            }
        }
    }
}
