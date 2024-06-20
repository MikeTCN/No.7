using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByTA : MonoBehaviour
{
    public float rotateSpeed = 10.0f;  // Ĭ����ת�ٶ�
    public Transform target;  // �����Transform

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
            // �Ƶ�����ת
            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);

            // ������Ϣ
            Debug.Log("Moon Position: " + transform.position);
            Debug.Log("Target (Earth) Position: " + target.position);
        }
    }
}
