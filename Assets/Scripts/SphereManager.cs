using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereManager : MonoBehaviour
{
    public static SphereManager instance;
    private int thrownCount = 0;
    public int requiredThrows = 2; // ��Ҫ�������������
    public SphereBehavior[] spheres; // �����������

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���Ӷ������������
    public void IncrementThrownCount()
    {
        thrownCount++;
        if (thrownCount >= requiredThrows)
        {
            TriggerRemainingSpheres();
        }
    }

    // ����ʣ�������߲���ʧ
    void TriggerRemainingSpheres()
    {
        foreach (SphereBehavior sphere in spheres)
        {
            if (sphere != null && !sphere.IsThrown)
            {
                sphere.FlyAndDisappearAutomatically();
            }
        }
    }
}
