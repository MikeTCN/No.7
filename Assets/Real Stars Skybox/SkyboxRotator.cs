using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class RotateSkyBox : MonoBehaviour
{
    // ������ÿ�������ת�Ƕ�
    public float rotationX;
    public float rotationY;
    public float rotationZ;

    void Update()
    {
        // �ڱ༭ģʽ����ת��պ�
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