using UnityEngine;
using System.Collections.Generic;

namespace PassthroughCameraSamples.MultiObjectDetection
{
    public class DetectedObjectLogger : MonoBehaviour
    {
    
        [SerializeField] private SentisInferenceUiManager inferenceUiManager;

        private List<BoundingBox> BoxDrawn => inferenceUiManager?.BoxDrawn;

        // Update is called once per frame
        void Update()
        {
            foreach (var box in BoxDrawn) {
                Debug.Log($"Detcted Object {box.ClassName} at: {box.WorldPos}");
            }
        }
    }
}


