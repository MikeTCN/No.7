using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animation animationComponent;
    public string animationName;

    private void Start()
    {
        // 播放動畫
        animationComponent.Play(animationName);
    }

    private void Update()
    {
        // 檢查動畫是否正在播放並且已經到達最後一幀
        if (animationComponent.IsPlaying(animationName) && animationComponent[animationName].time >= animationComponent[animationName].length)
        {
            // 停止動畫播放
            animationComponent.Stop(animationName);
        }
    }
}