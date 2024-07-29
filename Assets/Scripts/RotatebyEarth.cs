using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundTarget : MonoBehaviour
{
    public Transform target; // 旋转中心点
    public float rotateSpeed = 10f; // 旋转速度

    // 定义旋转轴的枚举
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    // 选择的旋转轴
    public RotationAxis rotationAxis = RotationAxis.Y;

    void Update()
    {
        Vector3 axis;

        // 根据选择的旋转轴设置旋转向量
        switch (rotationAxis)
        {
            case RotationAxis.X:
                axis = Vector3.right;
                break;
            case RotationAxis.Y:
                axis = Vector3.up;
                break;
            case RotationAxis.Z:
                axis = Vector3.forward;
                break;
            default:
                axis = Vector3.up;
                break;
        }

        // 执行旋转
        transform.RotateAround(target.position, axis, rotateSpeed * Time.deltaTime);
    }
}
