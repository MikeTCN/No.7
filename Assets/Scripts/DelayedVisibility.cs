using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedVisibility : MonoBehaviour
{
    public GameObject targetObject;  // ��Ҫ���ƿɼ��ԵĶ���
    public float delayTime = 5.0f;   // ��ʼ�ӳ�ʱ��
    public float visibleDuration = 10.0f; // �ɼ�����ʱ��

    void Start()
    {
        // ����Ŀ�����
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }

        // ����Э�����ӳ�ʱ�����ʾ����
        StartCoroutine(ShowAndHideObject());
    }

    IEnumerator ShowAndHideObject()
    {
        // �ȴ���ʼ�ӳ�ʱ��
        yield return new WaitForSeconds(delayTime);

        // ��ʾĿ�����
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }

        // �ȴ��ɼ�����ʱ��
        yield return new WaitForSeconds(visibleDuration);

        // ����Ŀ�����
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}