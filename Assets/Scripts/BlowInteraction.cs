using UnityEngine;
using UnityEngine.XR.Hands;

public class HandBlowInteraction : MonoBehaviour
{
    public XRHand hand; // XRHand 引用
    public Transform blowZone;
    public float blowZoneRadius = 0.2f;
    public Animator targetAnimator;
    public string animationParameterName = "IsBlowing"; // 改為動畫參數名稱
    public bool useAnimatorTrigger = false; // 是否使用觸發器而不是布爾值

    public float fingerCurlThreshold = 0.1f; // 手指彎曲閾值
    public float cooldownTime = 1f; // 冷卻時間，防止連續觸發

    [Header("Microphone Settings")]
    public float micSensitivity = 100; // 麥克風靈敏度
    public float loudness = 0; // 當前音量大小
    public float blowThreshold = 0.1f; // 吹氣音量閾值

    private bool isInBlowZone = false;
    private float lastBlowTime = -1f;
    private AudioClip microphoneClip;
    private string currentMicrophoneDevice;

    private void Start()
    {
        InitializeMicrophone();
    }

    private void Update()
    {
        if (hand == null || !hand.isTracked)
        {
            return;
        }

        XRHandJoint palmJoint = hand.GetJoint(XRHandJointID.Palm);
        if (palmJoint.TryGetPose(out var palmPose))
        {
            isInBlowZone = Vector3.Distance(palmPose.position, blowZone.position) <= blowZoneRadius;

            UpdateLoudness();

            if (isInBlowZone && Time.time - lastBlowTime > cooldownTime)
            {
                if (IsBlowingGesture() && IsBlowingSound())
                {
                    TriggerBlowAnimation();
                    Debug.Log("吹氣動作觸發！");
                    lastBlowTime = Time.time;
                }
                else
                {
                    StopBlowAnimation();
                }
            }
            else
            {
                StopBlowAnimation();
            }
        }
    }

    private bool IsBlowingGesture()
    {
        if (!hand.isTracked)
        {
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
            return avgDistance < fingerCurlThreshold;
        }

        return false;
    }

    private void InitializeMicrophone()
    {
        if (Microphone.devices.Length > 0)
        {
            currentMicrophoneDevice = Microphone.devices[0]; // 使用第一個可用的麥克風
            microphoneClip = Microphone.Start(currentMicrophoneDevice, true, 1, AudioSettings.outputSampleRate);
        }
        else
        {
            Debug.LogError("沒有找到麥克風設備！");
        }
    }

    private void UpdateLoudness()
    {
        if (microphoneClip != null)
        {
            float[] waveData = new float[1024];
            int micPosition = Microphone.GetPosition(currentMicrophoneDevice) - (1024 + 1);
            if (micPosition < 0) return;
            microphoneClip.GetData(waveData, micPosition);

            float wavePeak = 0f;
            for (int i = 0; i < 1024; i++)
            {
                float waveSample = waveData[i] * waveData[i];
                if (waveSample > wavePeak)
                {
                    wavePeak = waveSample;
                }
            }
            loudness = Mathf.Sqrt(wavePeak) * micSensitivity;
        }
    }

    private bool IsBlowingSound()
    {
        return loudness > blowThreshold;
    }

    private void TriggerBlowAnimation()
    {
        if (useAnimatorTrigger)
        {
            targetAnimator.SetTrigger(animationParameterName);
        }
        else
        {
            targetAnimator.SetBool(animationParameterName, true);
        }
    }

    private void StopBlowAnimation()
    {
        if (!useAnimatorTrigger)
        {
            targetAnimator.SetBool(animationParameterName, false);
        }
    }

    private void OnDisable()
    {
        if (Microphone.IsRecording(currentMicrophoneDevice))
        {
            Microphone.End(currentMicrophoneDevice);
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