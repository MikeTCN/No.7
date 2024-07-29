using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundTarget : MonoBehaviour
{
    public Transform target; // ��ת���ĵ�
    public float rotateSpeed = 10f; // ��ת�ٶ�

    // ������ת���ö��
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    // ѡ�����ת��
    public RotationAxis rotationAxis = RotationAxis.Y;

    void Update()
    {
        Vector3 axis;

        // ����ѡ�����ת��������ת����
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

        // ִ����ת
        transform.RotateAround(target.position, axis, rotateSpeed * Time.deltaTime);
    }
}
