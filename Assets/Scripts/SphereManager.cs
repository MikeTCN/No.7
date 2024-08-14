using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SphereManager : MonoBehaviour
{
    public static SphereManager instance;
    private int thrownCount = 0;
    public int requiredThrows = 2; // 需要丢出的球的数量
    public SphereBehavior[] spheres; // 所有球的引用

    public PlayableDirector finalTimeline2; // 引用FinalTimeline2

    public float flySpeed = 5f; // 球飞出去的速度，暴露为可调变量
    public bool areSpheresFlownAway = false; // 变量，用于检测球是否已经飞走

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 增加丢出的球的数量
    public void IncrementThrownCount()
    {
        thrownCount++;
        if (thrownCount >= requiredThrows)
        {
            StartCoroutine(TriggerRemainingSpheresWithDelay());
            PlayFinalTimeline(); // 计数达到设定值时，播放时间线
        }
    }

    // 带有1秒延迟触发球飞走的协程
    private IEnumerator TriggerRemainingSpheresWithDelay()
    {
        yield return new WaitForSeconds(1f); // 添加1秒延迟

        foreach (SphereBehavior sphere in spheres)
        {
            if (sphere != null && !sphere.IsThrown)
            {
                sphere.FlyAndDisappearAutomatically(flySpeed); // 传递可调速度
            }
        }

        areSpheresFlownAway = true; // 在球飞走之后，将变量设置为true
    }

    // 播放FinalTimeline2
    void PlayFinalTimeline()
    {
        if (finalTimeline2 != null)
        {
            finalTimeline2.Play();
            Debug.Log("FinalTimeline2 has been played.");
        }
        else
        {
            Debug.LogWarning("FinalTimeline2 is not assigned.");
        }
    }
}