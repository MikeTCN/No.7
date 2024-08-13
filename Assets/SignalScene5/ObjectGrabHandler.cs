using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectGrabController : MonoBehaviour
{
    public XRGrabInteractable[] objectsToMonitor; // ��Ҫ����Ƿ�ץ�յ�����
    public GameObject[] objectsToAppear; // Ҫ���ֵ�����
    public Vector3[] targetScales; // ÿ�������Ŀ�����ű���
    public float scaleSpeed = 1f; // �Ŵ��ٶ�

    private void Start()
    {
        // ��ʼ��ʱ����������С�����ɼ�״̬
        for (int i = 0; i < objectsToAppear.Length; i++)
        {
            objectsToAppear[i].transform.localScale = Vector3.zero;
            objectsToAppear[i].SetActive(false); // ���ض���
        }

        // Ϊÿ����Ҫ�����������ץȡ�¼�������
        foreach (XRGrabInteractable grabInteractable in objectsToMonitor)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
        }
    }

    // �����屻ץסʱ����
    private void OnGrab(SelectEnterEventArgs args)
    {
        StartCoroutine(ShowAndScaleObjects());
    }

    // �Ŵ������Э��
    private IEnumerator ShowAndScaleObjects()
    {
        for (int i = 0; i < objectsToAppear.Length; i++)
        {
            GameObject obj = objectsToAppear[i];
            obj.SetActive(true); // ��ʾ����
            Vector3 initialScale = Vector3.zero;
            Vector3 targetScale = targetScales[i]; // ʹ�����õ�Ŀ�����ű���
            float progress = 0f;

            while (progress < 1f)
            {
                progress += Time.deltaTime * scaleSpeed;
                obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, progress);
                yield return null;
            }

            obj.transform.localScale = targetScale; // ȷ�����մﵽĿ���С
        }
    }
}