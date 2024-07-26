using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit;

public class HandBlowInteraction : MonoBehaviour
{
    public XRHand hand; // XRHand 引用
    public Transform blowZone;
    public float blowZoneRadius = 0.2f;
    public Animator targetAnimator;
    public string animationTriggerName = "BlowTrigger";

    public float fingerCurlThreshold = 0.05f; // 手指彎曲的閾值，可能需要調整
    public float cooldownTime = 1f; // 冷卻時間，防止連續觸發

    private bool isInBlowZone = false;
    private float lastBlowTime = -1f;

    private void Update()
    {
        if (!hand.isTracked)
            return;

        // 獲取手掌位置
        XRHandJoint palmJoint = hand.GetJoint(XRHandJointID.Palm);
        if (palmJoint.TryGetPose(out var palmPose))
        {
            // 檢查手是否在吹氣區域內
            isInBlowZone = Vector3.Distance(palmPose.position, blowZone.position) <= blowZoneRadius;

            if (isInBlowZone && Time.time - lastBlowTime > cooldownTime)
            {
                // 檢查手的姿勢是否為十指緊扣
                if (IsFingerInterlockGesture())
                {
                    // 觸發動畫
                    targetAnimator.SetTrigger(animationTriggerName);
                    Debug.Log("吹氣動作觸發！");
                    lastBlowTime = Time.time;
                }
            }
        }
    }

    private bool IsFingerInterlockGesture()
    {
        if (!hand.isTracked)
            return false;

        // 獲取每個手指的末端關節
        XRHandJoint thumbTip = hand.GetJoint(XRHandJointID.ThumbTip);
        XRHandJoint indexTip = hand.GetJoint(XRHandJointID.IndexTip);
        XRHandJoint middleTip = hand.GetJoint(XRHandJointID.MiddleTip);
        XRHandJoint ringTip = hand.GetJoint(XRHandJointID.RingTip);
        XRHandJoint pinkyTip = hand.GetJoint(XRHandJointID.LittleTip);

        // 獲取手掌中心
        XRHandJoint palmCenter = hand.GetJoint(XRHandJointID.Palm);

        // 計算所有指尖到手掌中心的平均距離
        float avgDistance = (Vector3.Distance(thumbTip.TryGetPose(out var thumbPose) ? thumbPose.position : Vector3.zero, palmCenter.TryGetPose(out var palmPose) ? palmPose.position : Vector3.zero) +
                             Vector3.Distance(indexTip.TryGetPose(out var indexPose) ? indexPose.position : Vector3.zero, palmCenter.TryGetPose(out var _) ? palmPose.position : Vector3.zero) +
                             Vector3.Distance(middleTip.TryGetPose(out var middlePose) ? middlePose.position : Vector3.zero, palmCenter.TryGetPose(out var _) ? palmPose.position : Vector3.zero) +
                             Vector3.Distance(ringTip.TryGetPose(out var ringPose) ? ringPose.position : Vector3.zero, palmCenter.TryGetPose(out var _) ? palmPose.position : Vector3.zero) +
                             Vector3.Distance(pinkyTip.TryGetPose(out var pinkyPose) ? pinkyPose.position : Vector3.zero, palmCenter.TryGetPose(out var _) ? palmPose.position : Vector3.zero)) / 5f;

        // 如果平均距離小於某個閾值，我們認為手指是緊扣的
        return avgDistance < fingerCurlThreshold;
    }

    // 可視化吹氣區域（僅在編輯器中）
    private void OnDrawGizmos()
    {
        if (blowZone != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(blowZone.position, blowZoneRadius);
        }
    }
}