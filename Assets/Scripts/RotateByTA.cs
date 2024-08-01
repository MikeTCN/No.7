using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTA : MonoBehaviour
{
    public float rotateSpeed = 10.0f;  // 默认旋转速度
    public Transform target;  // 地球的Transform
    public bool clockwise = true;  // 控制旋转方向，默认顺时针

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
            // 根据方向控制旋转
            float direction = clockwise ? 1.0f : -1.0f;
            transform.RotateAround(target.position, Vector3.up, direction * rotateSpeed * Time.deltaTime);

            // 调试信息
            Debug.Log("Moon Position: " + transform.position);
            Debug.Log("Target (Earth) Position: " + target.position);
        }
    }
}
