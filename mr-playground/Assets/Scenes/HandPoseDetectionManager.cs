using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Oculus.Interaction.Input;
using PassthroughCameraSamples.MultiObjectDetection;
using Oculus.Interaction.HandGrab;

public class HandPoseDetectionManager : MonoBehaviour
{
    [SerializeField] private Hand leftHand;
    [SerializeField] private Hand rightHand;
    [SerializeField] private ObjectTrackerManager tracker;
    [SerializeField] private WallObjectSpawner spawner;
    [SerializeField] private float maxDistance = 0.1f;
    [SerializeField] private float deleteAfterSeconds = 5f;
    private Pose currentPoseLeft;
    private Pose currentPoseRight;
    private HandJointId handJointId = HandJointId.HandIndex3; // Index Finger Tip
    private float lastPoseTime = 0f;

    void Start()
    {

    }

    void Update()
    {
        leftHand.GetJointPose(handJointId, out currentPoseLeft);
        rightHand.GetJointPose(handJointId, out currentPoseRight);

        var currentTime = Time.time;
        if (currentTime - lastPoseTime > deleteAfterSeconds)
        {
            spawner.DestroyObject();
        }
    }

    public void PoseDetectedLeft()
    {
        PoseDetected(currentPoseLeft);
    }

    public void PoseDetectedRight()
    {
        PoseDetected(currentPoseRight);
    }

    private void PoseDetected(Pose pose)
    {
        Debug.Log($"Hand Position Detected at: {pose.position}");

        foreach (var obj in tracker.GetTrackedObjects())
        {
            var distance = Vector3.Distance(pose.position, obj.WorldPos);
            if (distance < maxDistance)
            {
                Debug.Log($"Tracked Object {obj.ClassName} at: {obj.WorldPos} matching with Hand at: {pose.position}");
                spawner.DestroyObject();
                spawner.SpawnObject(obj.ClassName);
                lastPoseTime = Time.time;
                return;
            }
        }
    }
}
