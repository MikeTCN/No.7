using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereInteraction : MonoBehaviour
{
    private bool isInteractable = false; // �Ƿ���Ի���
    public float flySpeed = 10f; // �����ٶ�
    public float flyTime = 2f; // ����ʱ��

    // ���û�������
    public void EnableInteraction()
    {
        isInteractable = true;
    }

    // �����ײ�¼�
    void OnTriggerEnter(Collider other)
    {
        if (isInteractable && other.CompareTag("Player"))
        {
            StartCoroutine(FlyAndDisappear(other.transform));
        }
    }

    // ����Sphere���в���ʧ
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

        Destroy(gameObject); // ɾ��Sphere����
    }
}