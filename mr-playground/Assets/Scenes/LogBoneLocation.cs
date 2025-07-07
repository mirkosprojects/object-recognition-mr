 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using TMPro;
 using Oculus.Interaction.Input;

 public class LogBoneLocation : MonoBehaviour
 {
     [SerializeField]
     private Hand hand;
     private Pose currentPose;
     private HandJointId handJointId = HandJointId.HandIndex3; // TO DO: Change this to your bone.

     void Update()
     {
         hand.GetJointPose(handJointId, out currentPose);
         Debug.LogError($"Hand Index Finger Pose: {currentPose.position}");
     }
 }