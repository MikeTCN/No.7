using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StarMarker : MonoBehaviour
{
    public Transform secondSkybox; // �ڶ�����պ�
    public float skyboxRadius = 50f; // �ڶ�����պеİ뾶

    void Start()
    {
        // ȷ��StarMarker��������ڵڶ�����պеı�����
        PlaceOnSkyboxSurface();
    }

    void Update()
    {
        // �ڲ���ģʽ�ͱ༭ģʽ�¶�ȷ�������ڱ�����
        PlaceOnSkyboxSurface();
    }

    void PlaceOnSkyboxSurface()
    {
        if (secondSkybox == null)
        {
            Debug.LogWarning("SecondSkybox is not assigned.");
            return;
        }

        Vector3 direction = (transform.position - secondSkybox.position).normalized;
        transform.position = secondSkybox.position + direction * skyboxRadius;
    }

    void OnDrawGizmos()
    {
        // �ڱ༭ģʽ��ʵʱ����λ��
        if (!Application.isPlaying)
        {
            PlaceOnSkyboxSurface();
        }
    }
}