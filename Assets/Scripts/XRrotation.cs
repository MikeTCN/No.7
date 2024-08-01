using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRrotation : MonoBehaviour
{
    public float targetHeight = 42.0f; // Ŀ��߶�
    public float rotationDuration = 1.0f; // ��ת�ٶ�
    private bool isstarted = false;
    public Transform cameraTransform;

    

    void Update()
    {
        // �������ĸ߶�
        if (cameraTransform.position.y >= targetHeight && !isstarted)
        {
            // ��ʼ��ת
            isstarted = true;
            cameraTransform.DORotate(new Vector3(0, 0, 0), rotationDuration).SetEase(Ease.InOutSine);
        }

       
    }
}
