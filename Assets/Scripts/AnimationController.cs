using UnityEngine;

public class CarAnimationController : MonoBehaviour
{
    public GameObject car;
    private Animator animator;
    private float animationLength;
    private float animationStartTime;
    private bool isAnimationPlaying = false;

    private void Start()
    {
        animator = car.GetComponent<Animator>();
        if (animator != null)
        {
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                animationLength = clipInfo[0].clip.length;
            }
            else
            {
                Debug.LogWarning("無法獲取動畫片段信息。請確保汽車對象有動畫片段。");
            }
        }
        else
        {
            Debug.LogError("汽車對象上沒有找到 Animator 組件！");
        }
    }

    private void Update()
    {
        if (isAnimationPlaying && Time.time - animationStartTime > animationLength)
        {
            StopAnimation();
        }
    }

    public void TriggerAnimation()
    {
        if (!isAnimationPlaying && animator != null)
        {
            animator.enabled = true;
            animator.speed = 1;
            animationStartTime = Time.time;
            isAnimationPlaying = true;
            Debug.Log("動畫開始播放");
        }
    }

    private void StopAnimation()
    {
        if (animator != null)
        {
            animator.speed = 0;
            isAnimationPlaying = false;
            Debug.Log("動畫停止");
        }
    }
}