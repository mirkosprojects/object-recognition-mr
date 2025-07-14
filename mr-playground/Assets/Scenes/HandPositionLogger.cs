 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using TMPro;
 using Oculus.Interaction.Input;
using Oculus.Interaction.PoseDetection;


public class HandPositionLogger : MonoBehaviour
{
    [SerializeField] private Hand hand;
    [SerializeField] private FingerFeatureStateThresholds ThumbFeatureTresholds;
    [SerializeField] private FingerFeatureStateThresholds IndexFeatureTresholds;
    [SerializeField] private FingerFeatureStateThresholds MiddleFeatureTresholds;
    [SerializeField] private FingerFeatureStateThresholds DefaultFeatureTresholds; // Ring & pinky finger
    [SerializeField] private Pose currentPose;

    private HandJointId handJointId = HandJointId.HandIndex3; // TO DO: Change this to your bone.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hand.GetJointPose(handJointId, out currentPose);
        LogFingerStates();
    }

    public void Log()
    {
        Debug.Log($"Hand Position Detected at: {currentPose.position}");
        LogFingerStates();
    }

    public void LogFingerStates()
    {
        // Iterate through each finger
        for (int fingerIndex = 0; fingerIndex < 5; fingerIndex++)
        {
            // Define the finger joints for each finger
            HandJointId[] fingerJoints = GetFingerJoints((HandFinger)fingerIndex);

            // Iterate through each joint in the finger
            foreach (HandJointId jointId in fingerJoints)
            {
                if (hand.GetJointPose(jointId, out Pose currentPose))
                {
                    Debug.Log($"Finger: {(HandFinger)fingerIndex}, Joint: {jointId}, Position: {currentPose.position}, Rotation: {currentPose.rotation}");
                }
            }
        }
    }

    private HandJointId[] GetFingerJoints(HandFinger finger)
    {
        // Return the joints for the specified finger
        switch (finger)
        {
            case HandFinger.Thumb:
                return new HandJointId[] { HandJointId.HandThumbTip, HandJointId.HandThumb3, HandJointId.HandThumb2, HandJointId.HandThumb3 };
            case HandFinger.Index:
                return new HandJointId[] { HandJointId.HandIndexTip, HandJointId.HandIndex3, HandJointId.HandIndex2, HandJointId.HandIndex1, HandJointId.HandIndex0 };
            case HandFinger.Middle:
                return new HandJointId[] { HandJointId.HandMiddleTip, HandJointId.HandMiddle3, HandJointId.HandMiddle2, HandJointId.HandMiddle1, HandJointId.HandMiddle0 };
            case HandFinger.Ring:
                return new HandJointId[] { HandJointId.HandRingTip, HandJointId.HandRing3, HandJointId.HandRing2, HandJointId.HandRing1, HandJointId.HandRing0 };
            case HandFinger.Pinky:
                return new HandJointId[] { HandJointId.HandPinkyTip, HandJointId.HandPinky3, HandJointId.HandPinky2, HandJointId.HandPinky1, HandJointId.HandPinky0 };
            default:
                return new HandJointId[0];
        }
    }

    //public void LogFingerStates()
    //{
    //    //// Iterate through each finger
    //    //for (int fingerIndex = 0; fingerIndex < 5; fingerIndex++)
    //    //{
    //    //    HandFinger finger = (HandFinger)fingerIndex;
    //    //    string fingerName = finger.ToString();

    //    //    hand.GetJointPose

    //    //    // Log finger curl state
    //    //    float curl = hand.GetFingerCurl(finger);
    //    //    Debug.Log($"{fingerName} Curl: {GetCurlState(curl)}");

    //    //    // Log finger flexion state
    //    //    float flexion = hand.GetFingerFlexion(finger);
    //    //    Debug.Log($"{fingerName} Flexion: {GetFlexionState(flexion)}");

    //    //    // Log finger abduction state
    //    //    float abduction = hand.GetFingerAbduction(finger);
    //    //    Debug.Log($"{fingerName} Abduction: {GetAbductionState(abduction)}");

    //    //    // Log finger opposition state
    //    //    float opposition = hand.(finger);
    //    //    Debug.Log($"{fingerName} Opposition: {GetOppositionState(opposition)}");
    //    //}
    //}

    private string GetCurlState(float curlValue)
    {
        if (curlValue < 0.3f)
            return "Open";
        else if (curlValue < 0.7f)
            return "Neutral";
        else
            return "Closed";
    }

    private string GetFlexionState(float flexionValue)
    {
        if (flexionValue < 0.3f)
            return "Open";
        else if (flexionValue < 0.7f)
            return "Neutral";
        else
            return "Closed";
    }

    private string GetAbductionState(float abductionValue)
    {
        if (abductionValue < 0.3f)
            return "Open";
        else if (abductionValue < 0.7f)
            return "Neutral";
        else
            return "Closed";
    }

    private string GetOppositionState(float oppositionValue)
    {
        if (oppositionValue < 0.3f)
            return "Open";
        else if (oppositionValue < 0.7f)
            return "Neutral";
        else
            return "Closed";
    }
}



