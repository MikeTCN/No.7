using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR.Hands;

public class HandBlowInteraction : MonoBehaviour
{
    public XRHand hand; // XRHand 引用
    public Transform blowZone;
    public float blowZoneRadius = 0.1f; // 起始值設小一點
    public Animator targetAnimator;
    public string animationTriggerName = "BlowTrigger";

    public float fingerCurlThreshold = 0.1f; // 初始值設大一點
    public float cooldownTime = 1f;

    private bool isInBlowZone = false;
    private float lastBlowTime = -1f;

    private void Update()
    {
        if (hand == null)
        {
            Debug.LogError("Hand reference is not set!");
            return;
        }

        if (!hand.isTracked)
        {
            Debug.Log("Hand is not tracked");
            return;
        }

        XRHandJoint palmJoint = hand.GetJoint(XRHandJointID.Palm);
        if (palmJoint.TryGetPose(out var palmPose))
        {
            float distance = Vector3.Distance(palmPose.position, blowZone.position);
            Debug.Log($"Distance to blow zone: {distance}");
            isInBlowZone = distance <= blowZoneRadius;

            if (isInBlowZone)
            {
                Debug.Log("Hand is in blow zone");
                if (Time.time - lastBlowTime > cooldownTime)
                {
                    if (IsFingerInterlockGesture())
                    {
                        Debug.Log("Blow gesture triggered!");
                        if (targetAnimator != null)
                        {
                            targetAnimator.SetTrigger(animationTriggerName);
                        }
                        else
                        {
                            Debug.LogError("Target Animator is not set!");
                        }
                        lastBlowTime = Time.time;
                    }
                }
            }
        }
        else
        {
            Debug.Log("Failed to get palm pose");
        }
    }

    private bool IsFingerInterlockGesture()
    {
        if (!hand.isTracked)
        {
            Debug.Log("Hand is not tracked in gesture check");
            return false;
        }

        XRHandJoint thumbTip = hand.GetJoint(XRHandJointID.ThumbTip);
        XRHandJoint indexTip = hand.GetJoint(XRHandJointID.IndexTip);
        XRHandJoint middleTip = hand.GetJoint(XRHandJointID.MiddleTip);
        XRHandJoint ringTip = hand.GetJoint(XRHandJointID.RingTip);
        XRHandJoint pinkyTip = hand.GetJoint(XRHandJointID.LittleTip);
        XRHandJoint palmCenter = hand.GetJoint(XRHandJointID.Palm);

        if (!palmCenter.TryGetPose(out var palmPose))
        {
            Debug.Log("Failed to get palm pose in gesture check");
            return false;
        }

        float avgDistance = 0f;
        int validFingers = 0;

        void AddFingerDistance(XRHandJoint fingerTip)
        {
            if (fingerTip.TryGetPose(out var fingerPose))
            {
                avgDistance += Vector3.Distance(fingerPose.position, palmPose.position);
                validFingers++;
            }
        }

        AddFingerDistance(thumbTip);
        AddFingerDistance(indexTip);
        AddFingerDistance(middleTip);
        AddFingerDistance(ringTip);
        AddFingerDistance(pinkyTip);

        if (validFingers > 0)
        {
            avgDistance /= validFingers;
            Debug.Log($"Average finger distance: {avgDistance}");
            return avgDistance < fingerCurlThreshold;
        }
        else
        {
            Debug.Log("No valid finger poses found");
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        if (blowZone != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(blowZone.position, blowZoneRadius);
        }
    }
}