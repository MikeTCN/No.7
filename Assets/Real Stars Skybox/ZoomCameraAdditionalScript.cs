using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCameraAdditionalScript : MonoBehaviour
{
    public Transform raycastSource; // 用于发射射线的对象
    public GameObject plane; // 地平线上的Plane
    public GameObject objectToDeactivate1; // 需要关闭的物体1
    public GameObject objectToDeactivate2; // 需要关闭的物体2
    public MeshRenderer[] meshRenderersToActivate = new MeshRenderer[6]; // 需要激活的MeshRenderer

    private bool isScriptActivated = false;
    private bool actionsTriggered = false;

    void Update()
    {
        if (SextantRaycaster.isNorthStarLocked && !isScriptActivated)
        {
            ActivateScript();
            isScriptActivated = true; // 确保脚本只激活一次
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
        Ray ray = new Ray(raycastSource.position, raycastSource.forward); // 从射线发射源发射射线

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

        // 激活MeshRenderer
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
