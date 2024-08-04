using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StarMarker : MonoBehaviour
{
    public Transform secondSkybox; // 第二层天空盒
    public float skyboxRadius = 50f; // 第二层天空盒的半径

    void Start()
    {
        // 确保StarMarker对象放置在第二层天空盒的表面上
        PlaceOnSkyboxSurface();
    }

    void PlaceOnSkyboxSurface()
    {
        Vector3 direction = (transform.position - secondSkybox.position).normalized;
        transform.position = secondSkybox.position + direction * skyboxRadius;
    }

    void OnDrawGizmos()
    {
        // 在编辑模式下实时更新位置
        if (!Application.isPlaying)
        {
            PlaceOnSkyboxSurface();
        }
    }
}
