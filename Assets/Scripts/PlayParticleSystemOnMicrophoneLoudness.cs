using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticleSystemOnMicrophoneLoudness : MonoBehaviour
{
    public AudioLoudnessDetection detector;
    public ParticleSystem particles;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    public AudioSource audioSource; // 添加AudioSource

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness <= threshold && particles.isPlaying)
        {
            particles.Stop();
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        else if (loudness > threshold && particles.isStopped)
        {
            particles.Play();
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnDisable()
    {
        particles.Stop();
        audioSource.Stop(); // 在組件禁用時也停止AudioSource
    }
}
