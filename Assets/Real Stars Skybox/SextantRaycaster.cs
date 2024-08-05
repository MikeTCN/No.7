using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SextantRaycaster : MonoBehaviour
{
    public Camera sextantCamera; // �������
    public Transform[] starMarkers; // �߸�Sphere����
    public Transform northStar; // �����ǣ�Star8��
    public LineRenderer guideLine; // �ɼ���ָ������

    private bool[] starsActivated;
    private bool allStarsActivated = false;
    private bool isNorthStarLocked = false;

    void Start()
    {
        starsActivated = new bool[starMarkers.Length];
        // ȷ���������ǵ�Mesh Renderer��ʼΪ����״̬
        foreach (Transform starMarker in starMarkers)
        {
            starMarker.GetComponent<MeshRenderer>().enabled = false;
        }

        // ȷ��ָ�����߳�ʼΪ����״̬
        guideLine.enabled = false;
    }

    void Update()
    {
        if (!allStarsActivated)
        {
            CheckStarsActivation();
        }

        if (allStarsActivated && !isNorthStarLocked)
        {
            StartCoroutine(DrawGuideLine());
        }

        if (isNorthStarLocked)
        {
            LockLeftViewOnNorthStar();
        }
        else
        {
            CheckNorthStarAlignment();
        }
    }

    void CheckStarsActivation()
    {
        Ray ray = sextantCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // ������������ķ�������

        for (int i = 0; i < starMarkers.Length; i++)
        {
            if (!starsActivated[i])
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == starMarkers[i])
                    {
                        // �������ǵ�Mesh Renderer
                        MeshRenderer meshRenderer = starMarkers[i].GetComponent<MeshRenderer>();
                        if (!meshRenderer.enabled)
                        {
                            meshRenderer.enabled = true;
                            starsActivated[i] = true;
                            Debug.Log("Aligned and activated " + starMarkers[i].name);
                        }
                    }
                }
            }
        }

        // ����Ƿ��������Ƕ�������
        allStarsActivated = true;
        foreach (bool activated in starsActivated)
        {
            if (!activated)
            {
                allStarsActivated = false;
                break;
            }
        }
    }

    IEnumerator DrawGuideLine()
    {
        if (guideLine.enabled) yield break;

        // ��ȡStar6��Star7��λ��
        Vector3 star6Pos = starMarkers[5].position;
        Vector3 star7Pos = starMarkers[6].position;

        // ����ָ������
        guideLine.enabled = true;
        guideLine.positionCount = 2;

        float elapsedTime = 0f;
        float duration = 1f; // �������ӳ���ʱ��
        Vector3 startPosition = star7Pos;
        Vector3 endPosition = northStar.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);
            guideLine.SetPosition(0, startPosition);
            guideLine.SetPosition(1, currentPos);
            yield return null;
        }

        guideLine.SetPosition(0, startPosition);
        guideLine.SetPosition(1, endPosition);
        Debug.Log("Guide line drawn from Star6 and Star7 to North Star");
    }

    void CheckNorthStarAlignment()
    {
        Ray ray = sextantCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // ������������ķ�������

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == northStar)
            {
                isNorthStarLocked = true;
                Debug.Log("North Star Locked");
            }
        }
    }

    void LockLeftViewOnNorthStar()
    {
        // �������ӽ������ڱ�������
        sextantCamera.transform.LookAt(northStar.position);
    }
}