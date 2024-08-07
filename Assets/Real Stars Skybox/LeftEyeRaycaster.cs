using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftEyeRaycaster : MonoBehaviour
{
    public Camera leftEyeCamera; // 左眼摄像机
    public Transform horizonSphere; // 地平线上的Sphere
    public GameObject objectToActivate; // 需要激活的物体
    public GameObject objectToDeactivate1; // 需要关闭的物体1
    public GameObject objectToDeactivate2; // 需要关闭的物体2
    public float endDelay = 5.0f; // 结束场景的延迟时间

    private bool isHorizonLocked = false;
    private bool endTriggered = false;

    void Update()
    {
        if (SextantRaycaster.isNorthStarLocked && !isHorizonLocked)
        {
            CheckHorizonAlignment();
        }

        if (isHorizonLocked && !endTriggered)
        {
            endTriggered = true;
            StartCoroutine(EndSceneAfterDelay(endDelay)); // 延迟指定秒数后结束场景
        }
    }

    void CheckHorizonAlignment()
    {
        Ray ray = leftEyeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 从摄像机的中心发射射线

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == horizonSphere)
            {
                isHorizonLocked = true;
                Debug.Log("Horizon locked");
            }
        }
    }

    IEnumerator EndSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ActivateObject();
        DeactivateObjects();
    }

    void ActivateObject()
    {
        // 激活物体的MeshRenderer
        if (objectToActivate != null)
        {
            MeshRenderer meshRenderer = objectToActivate.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
                Debug.Log("Object activated");
            }
        }
    }

    void DeactivateObjects()
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
    }
}