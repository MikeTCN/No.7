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

    public float flySpeed = 5f; // ��ɳ�ȥ���ٶȣ���¶Ϊ�ɵ�����
    public bool areSpheresFlownAway = false; // ���������ڼ�����Ƿ��Ѿ�����

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
            StartCoroutine(TriggerRemainingSpheresWithDelay());
            PlayFinalTimeline(); // �����ﵽ�趨ֵʱ������ʱ����
        }
    }

    // ����1���ӳٴ�������ߵ�Э��
    private IEnumerator TriggerRemainingSpheresWithDelay()
    {
        yield return new WaitForSeconds(1f); // ���1���ӳ�

        foreach (SphereBehavior sphere in spheres)
        {
            if (sphere != null && !sphere.IsThrown)
            {
                sphere.FlyAndDisappearAutomatically(flySpeed); // ���ݿɵ��ٶ�
            }
        }

        areSpheresFlownAway = true; // �������֮�󣬽���������Ϊtrue
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