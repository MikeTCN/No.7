﻿using UnityEngine;
using UnityEngine.Events;

public class LightActivator : MonoBehaviour
{
    public GameObject lightObject; // 要啟動的燈光物件
    public TriggerEventOnMicrophoneLoudness microphoneTrigger; // 參照到 TriggerEventOnMicrophoneLoudness 腳本
    public AudioSource audioSource; // 要播放音效的AudioSource

    private void Start()
    {
        if (microphoneTrigger == null)
        {
            Debug.LogError("MicrophoneTrigger not assigned in LightActivator!");
            return;
        }

        if (lightObject == null)
        {
            Debug.LogError("Light object not assigned in LightActivator!");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource not assigned in LightActivator!");
            return;
        }

        // 確保燈光一開始是關閉的
        lightObject.SetActive(false);

        // 訂閱 MicrophoneTrigger 完成事件
        microphoneTrigger.OnTriggerComplete.AddListener(ActivateLight);
    }

    private void ActivateLight()
    {
        lightObject.SetActive(true);
        Debug.Log("Light activated!");

        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Sound effect played!");
        }
        else
        {
            Debug.LogWarning("AudioSource not assigned!");
        }
    }

    private void OnDisable()
    {
        // 取消訂閱事件，避免記憶體洩漏
        if (microphoneTrigger != null)
        {
            microphoneTrigger.OnTriggerComplete.RemoveListener(ActivateLight);
        }
    }
}