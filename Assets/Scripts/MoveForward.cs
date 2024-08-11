using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // 暴露的速度参数，可以在Inspector中调整
    public float speed = 1.0f;

    void Update()
    {
        // 每帧沿着物体的Y轴方向移动
        transform.position += transform.up * speed * Time.deltaTime;
    }
}