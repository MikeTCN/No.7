using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public VoiceoverController voiceoverController;

    void Start()
    {
        // 開始自動播放語音
        voiceoverController.StartTimeline();
    }

    void OnSomeEvent()
    {
        // 觸發特定語音
        voiceoverController.TriggerSpecificVoiceover(2);
    }
}