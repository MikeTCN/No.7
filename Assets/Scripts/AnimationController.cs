using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    public string animationName;

    private void Start()
    {
        // 播放動畫
        animator.Play(animationName);
    }

    private void Update()
    {
        // 檢查動畫是否正在播放並且已經到達最後一幀
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            // 停止動畫播放
            animator.Play(animationName, 0, 0f);
        }
    }
}