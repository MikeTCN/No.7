using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class RotateSkyBox : MonoBehaviour
{
    // 设置绕每个轴的旋转角度
    public float rotationX;
    public float rotationY;
    public float rotationZ;

    void Update()
    {
        // 在编辑模式下旋转天空盒
        if (!Application.isPlaying)
        {
            UpdateSkyboxRotation();
        }
    }

    void UpdateSkyboxRotation()
    {
        if (RenderSettings.skybox != null)
        {
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
            RenderSettings.skybox.SetVector("_Rotation", new Vector4(rotation.x, rotation.y, rotation.z, rotation.w));
        }
    }

    void OnValidate()
    {
        UpdateSkyboxRotation();
    }
}