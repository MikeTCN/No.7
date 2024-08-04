using System.Collections;
using UnityEngine;

public class MoveActivateAndPauseAnimator : MonoBehaviour
{
    public PlayParticleSystemOnMicrophoneLoudness soundDetector; // 引用聲音檢測腳本
    public GameObject objectToMove;
    public GameObject sphereToShrink;
    public Vector3 targetPosition = new Vector3(230.789993f, 38.5009995f, 272.664001f);
    public float shrinkSpeed = 0.1f;
    public float minSphereSize = 0.01f;

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
        if (soundDetector.particles.isPlaying) // 使用粒子系統的播放狀態來判斷是否檢測到聲音
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