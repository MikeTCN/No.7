using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkyboxRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f;  // Skybox��ת�ٶ�

    void Update()
    {
        // ��ȡ��ǰSkybox����ת�Ƕ�
        float currentRotation = RenderSettings.skybox.GetFloat("_Rotation");

        // ������ת�Ƕ�
        float newRotation = currentRotation + rotationSpeed * Time.deltaTime;

        // �����µ���ת�Ƕ�
        RenderSettings.skybox.SetFloat("_Rotation", newRotation);
    }
}

