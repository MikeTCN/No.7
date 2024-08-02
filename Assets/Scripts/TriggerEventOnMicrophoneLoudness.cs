using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.Events;

public class TriggerEventOnMicrophoneLoudness : MonoBehaviour
{
    public AudioLoudnessDetection detector;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    public AudioSource audioSource; // AudioSource component

    public UnityEvent customEvent; // Custom UnityEvent to trigger in response to microphone loudness
    public UnityEvent stopEvent; // Custom UnityEvent to stop other component when triggered

    private bool isStopped = false; // Flag to track if stop event has been triggered

    public AzureEffectsController azureEffectsController; // Reference to AzureEffectsController script

    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness > threshold && !isStopped)
        {
            customEvent.Invoke(); // Invoke the custom event when loudness exceeds the threshold
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            stopEvent.Invoke(); // Invoke the stop event to stop other components
            isStopped = true; // Set flag to true to indicate stop event has been triggered

            // Stop the AzureEffectsController script
            azureEffectsController.enabled = false;
        }
        else if (loudness <= threshold && isStopped)
        {
            isStopped = false; // Reset the flag when loudness falls below threshold
        }

        if (loudness <= threshold && audioSource.isPlaying)
        {
            audioSource.Stop(); // Stop audio source when loudness falls below threshold
        }
    }

    private void OnDisable()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}