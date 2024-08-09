using System.Collections;
using UnityEngine;

public class PlayParticleSystemOnMicrophoneLoudness : MonoBehaviour
{
    public AudioLoudnessDetection detector;
    public ParticleSystem particles;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    public AudioSource audioSource;

    private bool canPlayAudio = true;
    private float audioDuration = 3f;
    private float fadeOutDuration = 0.5f; // 淡出持續時間
    private bool isFirstUpdate = true; // 用於檢查是否是第一次更新

    void Start()
    {
        // 確保音頻源一開始是停止的
        audioSource.Stop();
    }

    void Update()
    {
        // 第一幀不執行任何操作
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            return;
        }

        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        // 控制粒子效果
        if (loudness > threshold)
        {
            if (!particles.isPlaying)
            {
                particles.Play();
            }

            // 控制音頻
            if (canPlayAudio)
            {
                StartCoroutine(PlayAudioWithFadeOut());
            }
        }
        else
        {
            if (particles.isPlaying)
            {
                particles.Stop();
            }
        }
    }

    IEnumerator PlayAudioWithFadeOut()
    {
        canPlayAudio = false;
        audioSource.Play();

        // 播放2.5秒
        yield return new WaitForSeconds(audioDuration - fadeOutDuration);

        // 淡出0.5秒
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // 恢復原始音量
        canPlayAudio = true;
    }

    private void OnDisable()
    {
        particles.Stop();
        audioSource.Stop();
    }
}