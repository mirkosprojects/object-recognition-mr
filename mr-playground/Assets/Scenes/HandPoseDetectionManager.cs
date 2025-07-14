using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Oculus.Interaction.Input;
using PassthroughCameraSamples.MultiObjectDetection;

public class HandPoseDetectionManager : MonoBehaviour
{
    [SerializeField]  private Hand hand;
    [SerializeField] private ObjectTrackerManager tracker;
    [SerializeField] float maxDistance = 0.1f;
    private Pose currentPose;
    private HandJointId handJointId = HandJointId.HandIndex3; // TO DO: Change this to your bone.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        hand.GetJointPose(handJointId, out currentPose);
    }

    public void PoseDetected()
    {
        Debug.Log($"Hand Position Detected at: {currentPose.position}");

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
