using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventOnMicrophoneLoudness : MonoBehaviour
{
    public AudioLoudnessDetection detector;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    public AudioSource audioSource;

    public UnityEvent[] customEvents;
    public UnityEvent[] newCustomEvents;

    public UnityEvent OnTriggerComplete; // 新增的事件，將在所有事件完成後觸發

    private bool isStartCoroutineFinished = false;
    private bool hasStartedCustomEvent = false;

    [SerializeField] private Material sailOptimizedMaterial;
    [SerializeField] private Material sailMaterial;
    private static readonly string VOR_STRENGTH = "_VorStrength";
    private static readonly string VOR_SPEED = "_VorSpeed";

    void Start()
    {
        initializeWeather();
        SetInitialShaderValues();
    }

    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
        Debug.Log(loudness);

        if (loudness > threshold && !hasStartedCustomEvent && isStartCoroutineFinished)
        {
            StartCoroutine(TriggerEventsWithDelay(newCustomEvents, 1.6f));
            StartCoroutine(TransitionShaderValues());
            hasStartedCustomEvent = true;
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
        isStartCoroutineFinished = true;

        // 在所有事件完成後觸發 OnTriggerComplete 事件
        OnTriggerComplete.Invoke();
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

    private void SetInitialShaderValues()
    {
        if (sailOptimizedMaterial != null)
        {
            sailOptimizedMaterial.SetFloat(VOR_STRENGTH, 1f);
            sailOptimizedMaterial.SetFloat(VOR_SPEED, 0.06f);
        }
        if (sailMaterial != null)
        {
            sailMaterial.SetFloat(VOR_STRENGTH, 1f);
            sailMaterial.SetFloat(VOR_SPEED, 0.06f);
        }
    }

    private IEnumerator TransitionShaderValues()
    {
        float duration = 2f; // 漸變持續時間，可以根據需要調整
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float strength = Mathf.Lerp(1f, 5f, t);
            float speed = Mathf.Lerp(0.06f, 3f, t);

            if (sailOptimizedMaterial != null)
            {
                sailOptimizedMaterial.SetFloat(VOR_STRENGTH, strength);
                sailOptimizedMaterial.SetFloat(VOR_SPEED, speed);
            }
            if (sailMaterial != null)
            {
                sailMaterial.SetFloat(VOR_STRENGTH, strength);
                sailMaterial.SetFloat(VOR_SPEED, speed);
            }

            yield return null;
        }
    }
}