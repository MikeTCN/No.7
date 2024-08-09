using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XROriginRetreat : MonoBehaviour
{
    public float startRetreatTime = 5.0f; // ��ʼ���˵�ʱ�䣬��λΪ��
    public float retreatSpeed = 1.0f; // ���˵��ٶȣ���λΪ��/��
    public float retreatDuration = 3.0f; // ���˵ĳ���ʱ�䣬��λΪ��

    private bool isRetreating = false;
    private float retreatEndTime;
    private Vector3 retreatDirection;

    void Start()
    {
        // ��ָ��ʱ���ʼ����
        Invoke("StartRetreat", startRetreatTime);
    }

    void StartRetreat()
    {
        isRetreating = true;
        retreatEndTime = Time.time + retreatDuration;

        // ���˵ķ���������������Ŷ���ĺ���-z�᷽��
        retreatDirection = -transform.forward;
    }

    void Update()
    {
        if (isRetreating)
        {
            if (Time.time <= retreatEndTime)
            {
                // ��ָ���ķ����Ϻ���
                transform.position += retreatDirection * retreatSpeed * Time.deltaTime;
            }
            else
            {
                // ֹͣ����
                isRetreating = false;
            }
        }
    }
}