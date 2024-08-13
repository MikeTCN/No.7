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
            TriggerRemainingSpheres();
            PlayFinalTimeline(); // 计数达到设定值时，播放时间线
        }
    }

    // 触发剩余的球飞走并消失
    void TriggerRemainingSpheres()
    {
        foreach (SphereBehavior sphere in spheres)
        {
            if (sphere != null && !sphere.IsThrown)
            {
                sphere.FlyAndDisappearAutomatically();
            }
        }
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
