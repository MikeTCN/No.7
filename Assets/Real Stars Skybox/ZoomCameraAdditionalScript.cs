using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCameraAdditionalScript : MonoBehaviour
{
    public Transform raycastSource; // ���ڷ������ߵĶ���
    public GameObject plane; // ��ƽ���ϵ�Plane
    public GameObject objectToDeactivate1; // ��Ҫ�رյ�����1
    public GameObject objectToDeactivate2; // ��Ҫ�رյ�����2
    public MeshRenderer[] meshRenderersToActivate = new MeshRenderer[6]; // ��Ҫ�����MeshRenderer

    private bool isScriptActivated = false;
    private bool actionsTriggered = false;

    void Update()
    {
        if (SextantRaycaster.isNorthStarLocked && !isScriptActivated)
        {
            ActivateScript();
            isScriptActivated = true; // ȷ���ű�ֻ����һ��
        }

        if (isScriptActivated && !actionsTriggered)
        {
            CheckPlaneHit();
        }
    }

    void ActivateScript()
    {
        Debug.Log("ZoomCameraAdditionalScript activated.");
    }

    void CheckPlaneHit()
    {
        Ray ray = new Ray(raycastSource.position, raycastSource.forward); // �����߷���Դ��������

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == plane.transform)
            {
                Debug.Log("Plane hit detected");
                TriggerActions();
                actionsTriggered = true;
            }
        }
    }

    void TriggerActions()
    {
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

        // ����MeshRenderer
        foreach (MeshRenderer meshRenderer in meshRenderersToActivate)
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
                Debug.Log(meshRenderer.gameObject.name + " MeshRenderer activated");
            }
        }
    }
}
