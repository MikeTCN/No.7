using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;


public class SphereBehavior : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public float flySpeed = 10f; // 飞行速度
    public float flyTime = 2f; // 飞行时间

    private bool wasThrown = false; // 标记是否被丢出

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease); // 监听丢出事件
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // 检查物体是被丢出还是放下
        //if (args.interactorObject is XRDirectInteractor)
        {
            Debug.Log("123");
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null )
            {
                wasThrown = true;
                StartCoroutine(FlyAndDisappear(rb.velocity));
            }
        }
    }

    IEnumerator FlyAndDisappear(Vector3 initialVelocity)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float elapsedTime = 0f;

        while (elapsedTime < flyTime)
        {
            rb.velocity = initialVelocity;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // 删除Sphere对象
    }
}
