using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTA : MonoBehaviour
{
    public float rotateSpeed = 10.0f;  // 默认旋转速度
    public Transform target;  // 地球的Transform

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // 绕地球旋转
            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);

            // 调试信息
            Debug.Log("Moon Position: " + transform.position);
            Debug.Log("Target (Earth) Position: " + target.position);
        }
    }
}
