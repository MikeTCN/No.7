using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LeftEyeToDo : MonoBehaviour
{
    public Camera leftEyeCamera; // 左眼摄像机
    public MeshRenderer[] meshRenderersToActivate = new MeshRenderer[6]; // 需要激活的MeshRenderer
    public GameObject objectToDeactivate1; // 需要关闭的物体1
    public GameObject objectToDeactivate2; // 需要关闭的物体2
    public float yRotationThreshold = 10.0f; // 触发操作的y轴旋转角度阈值

    private bool isNorthStarLocked = false;
    private bool isRotationYExceeded = false;

    void Update()
    {
        if (SextantRaycaster.isNorthStarLocked && !isRotationYExceeded)
        {
            CheckCameraRotationY();
        }

        if (isRotationYExceeded)
        {
            TriggerActions();
        }
    }

    void CheckCameraRotationY()
    {
        // 获取摄像机的当前y轴旋转角度
        float currentYRotation = leftEyeCamera.transform.rotation.eulerAngles.y;
        // 判断是否低于阈值
        if (currentYRotation < yRotationThreshold)
        {
            isRotationYExceeded = true;
            Debug.Log("Camera Y rotation is below threshold");
        }
    }

    void TriggerActions()
    {
        // 激活MeshRenderer
        foreach (MeshRenderer meshRenderer in meshRenderersToActivate)
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
                Debug.Log(meshRenderer.gameObject.name + " MeshRenderer activated");
            }
        }

        // 关闭物体1
        if (objectToDeactivate1 != null)
        {
            objectToDeactivate1.SetActive(false);
            Debug.Log("Object 1 deactivated");
        }

        // 关闭物体2
        if (objectToDeactivate2 != null)
        {
            objectToDeactivate2.SetActive(false);
            Debug.Log("Object 2 deactivated");
        }
    }
}