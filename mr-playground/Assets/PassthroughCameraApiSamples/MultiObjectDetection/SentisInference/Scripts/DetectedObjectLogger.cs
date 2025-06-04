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
                Debug.Log($"Detected Object {i} - World Position 1: {DetectedObjects[i]}");
            }

            foreach (var boxPos in DetectedObjects) 
            {
                Debug.Log($"Detected Object - World Position 2: {boxPos}");
            }
        }
    }
}


