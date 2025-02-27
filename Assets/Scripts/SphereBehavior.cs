using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;


public class SphereBehavior : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public float flySpeed = 10f; // 默认飞行速度
    public float flyTime = 2f; // 飞行时间
    public bool IsThrown { get; private set; } = false; // 标记是否被丢出

    private bool wasThrown = false; // 标记是否被丢出

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease); // 监听丢出事件
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // 检查物体是被丢出还是放下
        Debug.Log("123");
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            wasThrown = true;
            SphereManager.instance.IncrementThrownCount();
            StartCoroutine(FlyAndDisappear(rb.velocity));
        }
    }

    // 带有速度参数的飞行和消失逻辑
    public void FlyAndDisappearAutomatically(float customFlySpeed)
    {
        StartCoroutine(FlyAndDisappear(Vector3.up * customFlySpeed)); // 使用自定义速度向上飞走
    }

    // 自动飞行和消失的协程
    IEnumerator FlyAndDisappear(Vector3 initialVelocity)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float elapsedTime = 0f;

        while (elapsedTime < flyTime)
        {
            rb.velocity = initialVelocity; // 使用给定的速度
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 缩小球体
        yield return StartCoroutine(ShrinkAndDestroy());
        Destroy(gameObject); // 删除Sphere对象
    }

    // 缩小并摧毁球体
    IEnumerator ShrinkAndDestroy()
    {
        float shrinkTime = 2f; // 缩小的持续时间
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 0.01f; // 缩小100倍

        float elapsedTime = 0f;

        while (elapsedTime < shrinkTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / shrinkTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // 删除球体对象
    }
}
