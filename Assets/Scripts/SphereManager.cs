using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SphereManager : MonoBehaviour
{
    public static SphereManager instance;
    private int thrownCount = 0;
    public int requiredThrows = 2; // ��Ҫ�������������
    public SphereBehavior[] spheres; // �����������

    public PlayableDirector finalTimeline2; // ����FinalTimeline2

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
            PlayFinalTimeline(); // �����ﵽ�趨ֵʱ������ʱ����
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

    // ����FinalTimeline2
    void PlayFinalTimeline()
    {
        if (finalTimeline2 != null)
        {
            finalTimeline2.Play();
            Debug.Log("FinalTimeline2 has been played.");
        }
        else
        {
            Debug.LogWarning("FinalTimeline2 is not assigned.");
        }
    }
}
