using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkyboxRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f;  // Skybox旋转速度

    void Update()
    {
        // 获取当前Skybox的旋转角度
        float currentRotation = RenderSettings.skybox.GetFloat("_Rotation");

        // 增加旋转角度
        float newRotation = currentRotation + rotationSpeed * Time.deltaTime;

        // 设置新的旋转角度
        RenderSettings.skybox.SetFloat("_Rotation", newRotation);
    }
}

