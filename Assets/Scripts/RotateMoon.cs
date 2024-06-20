using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMoon : MonoBehaviour
{
    public float rotateSpeed = 10.0f;  // Ĭ����ת�ٶ�
    public Transform target;  // �����Transform

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target is not assigned.");
        }

        // ���������ʼλ�ã���������������ĳ�ʼ������5����λ
        transform.position = target.position + new Vector3(5, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // �Ƶ�����ת��ʹ��Vector3.upȷ����y����ת
            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);

            // ������Ϣ
            Debug.Log("Moon Position: " + transform.position);
            Debug.Log("Target (Earth) Position: " + target.position);
        }
    }
}
