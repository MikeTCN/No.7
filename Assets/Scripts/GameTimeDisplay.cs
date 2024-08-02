using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeText;  // Reference to the TextMeshProUGUI component

    void Start()
    {
        // 确保timeText已分配
        if (timeText == null)
        {
            Debug.LogError("Time Text component is not assigned.");
        }
    }

    void Update()
    {
        if (timeText != null)
        {
            // Update the text with the game time in seconds
            float gameTime = Time.time;
            timeText.text = "Game Time: " + gameTime.ToString("F1") + "s";
        }
    }
}