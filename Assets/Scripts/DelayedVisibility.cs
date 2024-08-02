using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedVisibility : MonoBehaviour
{
    public GameObject targetObject;  // 需要控制可见性的对象
    public float delayTime = 5.0f;   // 初始延迟时间
    public float visibleDuration = 10.0f; // 可见持续时间

    void Start()
    {
        // 隐藏目标对象
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }

        // 启动协程在延迟时间后显示对象
        StartCoroutine(ShowAndHideObject());
    }

    IEnumerator ShowAndHideObject()
    {
        // 等待初始延迟时间
        yield return new WaitForSeconds(delayTime);

        // 显示目标对象
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }

        // 等待可见持续时间
        yield return new WaitForSeconds(visibleDuration);

        // 隐藏目标对象
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}