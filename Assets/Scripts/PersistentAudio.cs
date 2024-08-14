using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentAudio : MonoBehaviour
{
    private static PersistentAudio instance = null;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // ������Ƶ�ڳ����л�ʱ��������
        }
        else if (instance != this)
        {
            Destroy(gameObject);  // ����Ѿ���һ��ʵ�����ڣ������´�����ʵ��
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void StartPlayingFromMiddle()
    {
        if (audioSource != null)
        {
            audioSource.time = audioSource.clip.length / 2;  // ����Ƶ���м俪ʼ����
            audioSource.Play();
        }
    }
}