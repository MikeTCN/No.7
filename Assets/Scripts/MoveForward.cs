using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // ��¶���ٶȲ�����������Inspector�е���
    public float speed = 1.0f;

    void Update()
    {
        // ÿ֡���������Y�᷽���ƶ�
        transform.position += transform.up * speed * Time.deltaTime;
    }
}