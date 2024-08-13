using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class MoveActivateAndPauseAnimator : MonoBehaviour
{
    public PlayParticleSystemOnMicrophoneLoudness soundDetector;
    public GameObject objectToMove;
    public GameObject sphereToShrink;
    public Vector3 targetPosition = new Vector3(230.789993f, 38.5009995f, 272.664001f);
    public float shrinkSpeed = 0.1f;
    public float minSphereSize = 0.01f;
    public PlayableDirector timeline2Director; // 新增: Timeline2 的 PlayableDirector 引用

    private bool hasReachedTarget = false;
    private bool sphereDisappeared = false;
    private Animator objectAnimator;
    private float pauseDuration = 60f;

    void Start()
    {
        objectAnimator = objectToMove.GetComponent<Animator>();
        if (soundDetector == null)
        {
            Debug.LogError("請設置 PlayParticleSystemOnMicrophoneLoudness 腳本的引用！");
        }
        if (timeline2Director == null)
        {
            Debug.LogError("請設置 Timeline2 的 PlayableDirector 引用！");
        }
    }

    void Update()
    {
        if (!sphereDisappeared)
        {
            ShrinkSphereOnSound();
        }
        else if (!hasReachedTarget)
        {
            MoveObjectToTarget();
        }
        else
        {
            PauseAnimator();
        }
    }

    void ShrinkSphereOnSound()
    {
        if (soundDetector.particles.isPlaying)
        {
            Vector3 newScale = sphereToShrink.transform.localScale - Vector3.one * shrinkSpeed * Time.deltaTime;
            newScale = Vector3.Max(newScale, Vector3.one * minSphereSize);
            sphereToShrink.transform.localScale = newScale;

            if (newScale.x <= minSphereSize)
            {
                sphereDisappeared = true;
                sphereToShrink.SetActive(false);
            }
        }
    }

    void MoveObjectToTarget()
    {
        objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, targetPosition, Time.deltaTime);

        if (objectToMove.transform.position == targetPosition)
        {
            ActivateAnimator();
            hasReachedTarget = true;
        }
    }

    void ActivateAnimator()
    {
        if (objectAnimator != null)
        {
            objectAnimator.enabled = true;

            // 停止粒子系統的觸發
            if (soundDetector != null)
            {
                soundDetector.enabled = false;  // 禁用聲音檢測腳本
                soundDetector.particles.Stop(); // 停止粒子系統
                soundDetector.audioSource.Stop(); // 停止音頻源
            }

            // 播放 Timeline2 的音頻
            if (timeline2Director != null)
            {
                timeline2Director.Play();
                Debug.Log("Timeline2 開始播放");
            }
            else
            {
                Debug.LogError("Timeline2 Director 未設置！");
            }
        }
    }

    void PauseAnimator()
    {
        if (objectAnimator != null)
        {
            pauseDuration -= Time.deltaTime;
            if (pauseDuration <= 0)
            {
                objectAnimator.speed = 0;
            }
        }
    }
}