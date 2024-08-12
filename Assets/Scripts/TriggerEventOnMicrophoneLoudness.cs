using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine;
using System.Collections;

public class TriggerEventOnMicrophoneLoudness : MonoBehaviour
{
    public AudioLoudnessDetection detector;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    public AudioSource audioSource;

    public UnityEvent[] customEvents;
    public UnityEvent[] newCustomEvents;

    public UnityEvent OnTriggerComplete; // 所有事件完成后触发

    public PlayableDirector timeline2;  // 引用Timeline2

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
            StartCoroutine(PlayTimelineWithDelay());
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
        yield return new WaitForSeconds(38.0f);
        isStartCoroutineFinished = true;

        // 触发 OnTriggerComplete 事件
        OnTriggerComplete.Invoke();
    }

    private IEnumerator PlayTimelineWithDelay()
    {
        // 停顿1秒后播放 Timeline2
        yield return new WaitForSeconds(1.0f);

        if (timeline2 != null)
        {
            timeline2.Play();
            Debug.Log("Timeline2 started after 1-second delay.");
        }
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
        float duration = 2f; // 渐变持续时间，可以根据需要调整
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