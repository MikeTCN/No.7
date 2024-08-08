using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LeftEyeToDo : MonoBehaviour
{
    public Camera leftEyeCamera; // ���������
    public MeshRenderer[] meshRenderersToActivate = new MeshRenderer[6]; // ��Ҫ�����MeshRenderer
    public GameObject objectToDeactivate1; // ��Ҫ�رյ�����1
    public GameObject objectToDeactivate2; // ��Ҫ�رյ�����2
    public float yRotationThreshold = 10.0f; // ����������y����ת�Ƕ���ֵ

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
        // ��ȡ������ĵ�ǰy����ת�Ƕ�
        float currentYRotation = leftEyeCamera.transform.rotation.eulerAngles.y;
        // �ж��Ƿ������ֵ
        if (currentYRotation < yRotationThreshold)
        {
            isRotationYExceeded = true;
            Debug.Log("Camera Y rotation is below threshold");
        }
    }

    void TriggerActions()
    {
        // ����MeshRenderer
        foreach (MeshRenderer meshRenderer in meshRenderersToActivate)
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
                Debug.Log(meshRenderer.gameObject.name + " MeshRenderer activated");
            }
        }

        // �ر�����1
        if (objectToDeactivate1 != null)
        {
            objectToDeactivate1.SetActive(false);
            Debug.Log("Object 1 deactivated");
        }

        // �ر�����2
        if (objectToDeactivate2 != null)
        {
            objectToDeactivate2.SetActive(false);
            Debug.Log("Object 2 deactivated");
        }
    }
}