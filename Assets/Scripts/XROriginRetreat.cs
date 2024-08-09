using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XROriginRetreat : MonoBehaviour
{
    public float startRetreatTime = 5.0f; // 开始后退的时间，单位为秒
    public float retreatSpeed = 1.0f; // 后退的速度，单位为米/秒
    public float retreatDuration = 3.0f; // 后退的持续时间，单位为秒

    private bool isRetreating = false;
    private float retreatEndTime;
    private Vector3 retreatDirection;

    void Start()
    {
        // 在指定时间后开始后退
        Invoke("StartRetreat", startRetreatTime);
    }

    void StartRetreat()
    {
        isRetreating = true;
        retreatEndTime = Time.time + retreatDuration;

        // 后退的方向，这里假设是沿着对象的后向（-z轴方向）
        retreatDirection = -transform.forward;
    }

    void Update()
    {
        if (isRetreating)
        {
            if (Time.time <= retreatEndTime)
            {
                // 在指定的方向上后退
                transform.position += retreatDirection * retreatSpeed * Time.deltaTime;
            }
            else
            {
                // 停止后退
                isRetreating = false;
            }
        }
    }
}