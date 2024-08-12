using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using UnityEngine.XR.Hands.Samples.GestureSample; // 添加這個命名空間

public class TimelinePlayerForGesture : MonoBehaviour
{
    private bool hasPlayed = false;
    private PlayableDirector timeline2Director;

    [SerializeField]
    private StaticHandGesture targetGesture; // 在Inspector中指定目標手勢

    private void Awake()
    {
        GameObject timeline2Object = GameObject.Find("Timeline2");
        if (timeline2Object != null)
        {
            timeline2Director = timeline2Object.GetComponent<PlayableDirector>();
            if (timeline2Director == null)
            {
                Debug.LogError("Timeline2 object does not have a PlayableDirector component! 😱");
            }
        }
        else
        {
            Debug.LogError("Timeline2 object not found in the scene! 😱");
        }
    }

    private void Start()
    {
        if (timeline2Director != null)
        {
            timeline2Director.Pause();
            Debug.Log("Timeline2 paused at start. 🎬");
        }

        if (targetGesture != null)
        {
            targetGesture.gesturePerformed.AddListener(TriggerTimelinePlayback);
            Debug.Log("Listening for gesture performance. 👂");
        }
        else
        {
            Debug.LogError("Target gesture not set in the inspector! 😱");
        }
    }

    private void OnDisable()
    {
        if (targetGesture != null)
        {
            targetGesture.gesturePerformed.RemoveListener(TriggerTimelinePlayback);
        }
    }

    private void TriggerTimelinePlayback()
    {
        if (!hasPlayed && timeline2Director != null)
        {
            hasPlayed = true;
            StartCoroutine(PlayTimelineCoroutine());
        }
        else if (hasPlayed)
        {
            Debug.Log("Timeline已經觸發過，忽略此次請求。🔁");
        }
        else
        {
            Debug.LogError("Timeline2 Director not found or initialized! 😱");
        }
    }

    private IEnumerator PlayTimelineCoroutine()
    {
        timeline2Director.Evaluate();
        yield return null;
        timeline2Director.Play();
        Debug.Log("Timeline2 has been evaluated and played. 🎭");
    }

    public void ResetTimelineState()
    {
        if (timeline2Director != null)
        {
            hasPlayed = false;
            timeline2Director.Pause();
            timeline2Director.time = 0;
            Debug.Log("Timeline狀態已重置。🔄");
        }
    }
}