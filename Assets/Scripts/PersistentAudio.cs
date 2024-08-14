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
            DontDestroyOnLoad(gameObject);  // 保持音频在场景切换时不被销毁
        }
        else if (instance != this)
        {
            Destroy(gameObject);  // 如果已经有一个实例存在，销毁新创建的实例
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void StartPlayingFromMiddle()
    {
        if (audioSource != null)
        {
            audioSource.time = audioSource.clip.length / 2;  // 从音频的中间开始播放
            audioSource.Play();
        }
    }
}