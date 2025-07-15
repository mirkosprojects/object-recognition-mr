using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Oculus.Interaction.Input;
using PassthroughCameraSamples.MultiObjectDetection;

public class HandPoseDetectionManager : MonoBehaviour
{
    [SerializeField] private Hand hand;
    [SerializeField] private ObjectTrackerManager tracker;
    [SerializeField] private float maxDistance = 0.1f;
    [SerializeField] private GameObject cubePrefab; // Reference to the Cube prefab

    private Pose currentPose;
    private HandJointId handJointId = HandJointId.HandIndex3; // TO DO: Change this to your bone.

    void Start()
    {

    }

    void Update()
    {
        hand.GetJointPose(handJointId, out currentPose);
    }

    public void PoseDetected()
    {
        Debug.Log($"Hand Position Detected at: {currentPose.position}");

        // Instantiate a cube at the hand's position
        if (cubePrefab != null)
        {
            Instantiate(cubePrefab, currentPose.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Cube prefab is not assigned in the inspector.");
        }

        foreach (var obj in tracker.GetTrackedObjects())
        {
            var distance = Vector3.Distance(currentPose.position, obj.WorldPos);
            if (distance < maxDistance)
            {
                Debug.Log($"Tracked Object {obj.ClassName} at: {obj.WorldPos} matching with Hand at: {currentPose.position}");
            }
        }
    }
}
