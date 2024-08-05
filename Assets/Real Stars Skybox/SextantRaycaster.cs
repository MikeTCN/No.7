using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SextantRaycaster : MonoBehaviour
{
    public Camera sextantCamera; // 主摄像机
    public Transform[] starMarkers; // 七个Sphere对象
    public Transform northStar; // 北极星（Star8）
    public LineRenderer guideLine; // 可见的指引射线

    private bool[] starsActivated;
    private bool allStarsActivated = false;
    private bool isNorthStarLocked = false;

    void Start()
    {
        starsActivated = new bool[starMarkers.Length];
        // 确保所有星星的Mesh Renderer初始为禁用状态
        foreach (Transform starMarker in starMarkers)
        {
            starMarker.GetComponent<MeshRenderer>().enabled = false;
        }

        // 确保指引射线初始为禁用状态
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
        Ray ray = sextantCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 从摄像机的中心发射射线

        for (int i = 0; i < starMarkers.Length; i++)
        {
            if (!starsActivated[i])
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == starMarkers[i])
                    {
                        // 激活星星的Mesh Renderer
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

        // 检查是否所有星星都被激活
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

        // 获取Star6和Star7的位置
        Vector3 star6Pos = starMarkers[5].position;
        Vector3 star7Pos = starMarkers[6].position;

        // 设置指引射线
        guideLine.enabled = true;
        guideLine.positionCount = 2;

        float elapsedTime = 0f;
        float duration = 1f; // 引导线延长的时间
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
        Ray ray = sextantCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 从摄像机的中心发射射线

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
        // 将左半边视角锁定在北极星上
        sextantCamera.transform.LookAt(northStar.position);
    }
}