using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRrotation : MonoBehaviour
{
    public float targetHeight = 42.0f; // 目标高度
    public float rotationDuration = 1.0f; // 旋转速度
    private bool isstarted = false;
    public Transform cameraTransform;

    

    void Update()
    {
        // 检查相机的高度
        if (cameraTransform.position.y >= targetHeight && !isstarted)
        {
            // 开始旋转
            isstarted = true;
            cameraTransform.DORotate(new Vector3(0, 0, 0), rotationDuration).SetEase(Ease.InOutSine);
        }

       
    }
}
