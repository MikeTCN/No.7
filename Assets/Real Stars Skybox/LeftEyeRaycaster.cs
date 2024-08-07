using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftEyeRaycaster : MonoBehaviour
{
    public Camera leftEyeCamera; // ���������
    public Transform horizonSphere; // ��ƽ���ϵ�Sphere
    public GameObject objectToActivate; // ��Ҫ���������
    public GameObject objectToDeactivate1; // ��Ҫ�رյ�����1
    public GameObject objectToDeactivate2; // ��Ҫ�رյ�����2
    public float endDelay = 5.0f; // �����������ӳ�ʱ��

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
            StartCoroutine(EndSceneAfterDelay(endDelay)); // �ӳ�ָ���������������
        }
    }

    void CheckHorizonAlignment()
    {
        Ray ray = leftEyeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // ������������ķ�������

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
        // ���������MeshRenderer
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