using UnityEngine;
using UnityEngine.Playables;

public class VoiceoverController : MonoBehaviour
{
    public PlayableDirector timeline;
    public float[] triggerTimes; // 觸發時間點

    private bool[] triggered;

    void Start()
    {
        triggered = new bool[triggerTimes.Length];
    }

    void Update()
    {
        for (int i = 0; i < triggerTimes.Length; i++)
        {
            if (!triggered[i] && timeline.time >= triggerTimes[i])
            {
                TriggerVoiceover(i);
                triggered[i] = true;
            }
        }
    }

    public void StartTimeline()
    {
        timeline.Play();
    }

    public void PauseTimeline()
    {
        timeline.Pause();
    }

    public void ResumeTimeline()
    {
        timeline.Resume();
    }

    private void TriggerVoiceover(int index)
    {
        Debug.Log($"Triggered voiceover at index {index}");
        // 在這裡添加觸發邏輯，例如播放特定的音頻
    }

    // 用於外部觸發的方法
    public void TriggerSpecificVoiceover(int index)
    {
        if (index >= 0 && index < triggerTimes.Length)
        {
            timeline.time = triggerTimes[index];
            timeline.Evaluate();
            timeline.Play();
        }
    }
}