using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventOnMicrophoneLoudness : MonoBehaviour
{
    public AudioLoudnessDetection detector;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    public AudioSource audioSource; // AudioSource component

    public UnityEvent[] customEvents; // Array of custom UnityEvents to trigger in response to microphone loudness
    public UnityEvent[] newCustomEvents;

    private bool isStartCoroutineFinished = false; // Flag to track if coroutine has finished   
    private bool hasStartedCustomEvent = false; // Flag to track if custom event has started

    void Start()
    {
        initializeWeather();
    }

    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
        Debug.Log(loudness);

        if (loudness > threshold && !hasStartedCustomEvent && isStartCoroutineFinished)
        {
            StartCoroutine(TriggerEventsWithDelay(newCustomEvents, 1.6f));
            hasStartedCustomEvent = true; // Set flag to true to indicate custom event has started
        }

    }

    private IEnumerator TriggerEventsWithDelay(UnityEvent[] events, float delay)
    {
        for (int i = 0; i < events.Length; i++)
        {
            events[i].Invoke();
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(10.0f);
        isStartCoroutineFinished = true; // Set flag to true to indicate coroutine has finished
    }

    private void OnDisable()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void initializeWeather()
    {
        StartCoroutine(TriggerEventsWithDelay(customEvents, 0.2f));
    }
}