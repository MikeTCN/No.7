using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionController2 : MonoBehaviour
{
    public float jumpHeight = 100f;
    public float jumpSpeed;
    public float transitionTime;
    public int sceneToLoad;
    public SceneTransitionManager sceneTransitionManager;
    private Vector3 jumpStartPosition;
    public ParticleSystem cloudEffect;
    public float cloudEffectHeight = 80f;
    public AudioSource audioSource; // 新增：用於播放音頻

    private bool hasTransitioned = false;

    public void StartTransition()
    {
        if (!hasTransitioned)
        {
            StartCoroutine(TransitionCoroutine());
        }
        else
        {
            Debug.Log("轉場已經觸發過，忽略此次請求。");
        }
    }

    private IEnumerator TransitionCoroutine()
    {
        hasTransitioned = true;

        jumpStartPosition = transform.position;

        float jumpDuration = jumpHeight / jumpSpeed;
        float elapsedTime = 0f;

        bool cloudEffectTriggered = false;

        // 播放音頻
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio source is not assigned!");
        }

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float jumpAmount = Mathf.Lerp(0f, jumpHeight, t);
            Vector3 newPosition = jumpStartPosition + Vector3.up * jumpAmount;
            transform.position = newPosition;

            if (!cloudEffectTriggered && newPosition.y >= jumpStartPosition.y + cloudEffectHeight)
            {
                TriggerCloudEffect();
                cloudEffectTriggered = true;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 targetPosition = jumpStartPosition + Vector3.up * jumpHeight;
        elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            float t = elapsedTime / transitionTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(transitionTime);

        // 停止音頻
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        sceneTransitionManager.GoToSceneAsync(sceneToLoad);
    }

    private void TriggerCloudEffect()
    {
        if (cloudEffect != null)
        {
            cloudEffect.transform.position = transform.position;
            cloudEffect.Play();
        }
        else
        {
            Debug.LogWarning("Cloud effect particle system is not assigned!");
        }
    }

    public void ResetTransitionState()
    {
        hasTransitioned = false;
        Debug.Log("轉場狀態已重置。");
    }
}