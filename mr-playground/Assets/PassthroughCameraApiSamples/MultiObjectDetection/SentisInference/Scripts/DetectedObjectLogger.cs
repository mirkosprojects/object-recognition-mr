using UnityEngine;

namespace PassthroughCameraSamples.MultiObjectDetection
{
    public class DetectedObjectLogger : MonoBehaviour
    {
    
        [SerializeField] private SentisInferenceUiManager inferenceUiManager;

        private Vector3[] DetectedObjects => inferenceUiManager?.DetectedBoxes;

        // Update is called once per frame
        void Update()
        {
            if (DetectedObjects == null) {
                return;
            }

            for (int i = 0; i < DetectedObjects.Length; i++) {
                Debug.Log($"Detected Object {i} - World Position: {DetectedObjects[i]}");
            }
        }
    }
}


