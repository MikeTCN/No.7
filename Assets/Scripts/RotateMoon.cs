using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMoon : MonoBehaviour
{
    public float rotateSpeed = 10.0f;  // 默认旋转速度
    public Transform target;  // 地球的Transform

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target is not assigned.");
        }

        // 设置月球初始位置，假设月球距离地球的初始距离是5个单位
        transform.position = target.position + new Vector3(5, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // 绕地球旋转，使用Vector3.up确保绕y轴旋转
            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);

            // 调试信息
            Debug.Log("Moon Position: " + transform.position);
            Debug.Log("Target (Earth) Position: " + target.position);
        }
    }
}
