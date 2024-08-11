using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereInteraction : MonoBehaviour
{
    private bool isInteractable = false; // 是否可以互动
    public float flySpeed = 10f; // 飞行速度
    public float flyTime = 2f; // 飞行时间

    // 启用互动功能
    public void EnableInteraction()
    {
        isInteractable = true;
    }

    // 检测碰撞事件
    void OnTriggerEnter(Collider other)
    {
        if (isInteractable && other.CompareTag("Player"))
        {
            StartCoroutine(FlyAndDisappear(other.transform));
        }
    }

    // 控制Sphere飞行并消失
    IEnumerator FlyAndDisappear(Transform playerTransform)
    {
        Vector3 flyDirection = (transform.position - playerTransform.position).normalized;
        float elapsedTime = 0f;

        while (elapsedTime < flyTime)
        {
            transform.position += flyDirection * flySpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // 删除Sphere对象
    }
}