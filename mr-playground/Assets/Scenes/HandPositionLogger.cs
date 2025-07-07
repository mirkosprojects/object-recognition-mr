 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using TMPro;
 using Oculus.Interaction.Input;


public class HandPositionLogger : MonoBehaviour
{

    [SerializeField]
    private Hand hand;
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

    public void Log()
    {
        Debug.LogError($"Hand Position Detected at: {currentPose.position}");
    }
}



