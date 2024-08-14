using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;


public class SphereBehavior : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public float flySpeed = 10f; // Ĭ�Ϸ����ٶ�
    public float flyTime = 2f; // ����ʱ��
    public bool IsThrown { get; private set; } = false; // ����Ƿ񱻶���

    private bool wasThrown = false; // ����Ƿ񱻶���

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease); // ���������¼�
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // ��������Ǳ��������Ƿ���
        Debug.Log("123");
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            wasThrown = true;
            SphereManager.instance.IncrementThrownCount();
            StartCoroutine(FlyAndDisappear(rb.velocity));
        }
    }

    // �����ٶȲ����ķ��к���ʧ�߼�
    public void FlyAndDisappearAutomatically(float customFlySpeed)
    {
        StartCoroutine(FlyAndDisappear(Vector3.up * customFlySpeed)); // ʹ���Զ����ٶ����Ϸ���
    }

    // �Զ����к���ʧ��Э��
    IEnumerator FlyAndDisappear(Vector3 initialVelocity)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float elapsedTime = 0f;

        while (elapsedTime < flyTime)
        {
            rb.velocity = initialVelocity; // ʹ�ø������ٶ�
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // ��С����
        yield return StartCoroutine(ShrinkAndDestroy());
        Destroy(gameObject); // ɾ��Sphere����
    }

    // ��С���ݻ�����
    IEnumerator ShrinkAndDestroy()
    {
        float shrinkTime = 2f; // ��С�ĳ���ʱ��
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 0.01f; // ��С100��

        float elapsedTime = 0f;

        while (elapsedTime < shrinkTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / shrinkTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // ɾ���������
    }
}
