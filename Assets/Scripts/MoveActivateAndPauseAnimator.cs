using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class MoveActivateAndPauseAnimator : MonoBehaviour
{
    [Header("Sound and Particle Settings")]
    public PlayParticleSystemOnMicrophoneLoudness soundDetector;

    [Header("Object Movement Settings")]
    public GameObject objectToMove;
    public Vector3 targetPosition = new Vector3(230.789993f, 38.5009995f, 272.664001f);

    [Header("Sphere Shrink Settings")]
    public GameObject sphereToShrink;
    public float shrinkSpeed = 0.1f;
    public float minSphereSize = 0.01f;

    [Header("Animation Settings")]
    public PlayableDirector timeline2Director;
    private Animator objectAnimator;
    private float pauseDuration = 60f;

    [Header("Quad Fade In Settings")]
    public GameObject quadToFadeIn;
    public float fadeInDuration = 2f;
    private MeshRenderer quadRenderer;
    private Material quadMaterial;

    private bool hasReachedTarget = false;
    private bool sphereDisappeared = false;

    void Start()
    {
        objectAnimator = objectToMove.GetComponent<Animator>();

        if (soundDetector == null)
        {
            Debug.LogError("請設置 PlayParticleSystemOnMicrophoneLoudness 腳本的引用！⚠️");
        }

        if (timeline2Director == null)
        {
            Debug.LogError("請設置 Timeline2 的 PlayableDirector 引用！⚠️");
        }

        if (quadToFadeIn != null)
        {
            quadRenderer = quadToFadeIn.GetComponent<MeshRenderer>();
            if (quadRenderer != null)
            {
                quadMaterial = quadRenderer.material;
                SetQuadAlpha(0);
                quadRenderer.enabled = false;
            }
            else
            {
                Debug.LogError("Quad 沒有 MeshRenderer 組件！⚠️");
            }
        }
        else
        {
            Debug.LogError("請設置要淡入的 Quad！⚠️");
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
                Debug.Log("球體縮小完成，已隱藏 🔍");
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
            Debug.Log("物體已到達目標位置 🏁");
        }
    }

    void ActivateAnimator()
    {
        if (objectAnimator != null)
        {
            objectAnimator.enabled = true;

            if (soundDetector != null)
            {
                soundDetector.enabled = false;
                soundDetector.particles.Stop();
                soundDetector.audioSource.Stop();
                Debug.Log("聲音檢測和粒子系統已停止 🔇");
            }

            StartCoroutine(FadeInQuad());

            if (timeline2Director != null)
            {
                timeline2Director.Play();
                Debug.Log("Timeline2 開始播放 🎵");
            }
            else
            {
                Debug.LogError("Timeline2 Director 未設置！⚠️");
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
                Debug.Log("動畫已暫停 ⏸️");
            }
        }
    }

    IEnumerator FadeInQuad()
    {
        if (quadRenderer != null && quadMaterial != null)
        {
            quadRenderer.enabled = true;
            float elapsedTime = 0;
            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
                SetQuadAlpha(alpha);
                yield return null;
            }
            SetQuadAlpha(1);
            Debug.Log("Quad 淡入完成 🖼️");
        }
    }

    void SetQuadAlpha(float alpha)
    {
        Color color = quadMaterial.color;
        color.a = alpha;
        quadMaterial.color = color;
    }
}