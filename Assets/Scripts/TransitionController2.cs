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

    private bool hasTransitioned = false; // 新增：用於標記是否已經轉場

    public void StartTransition()
    {
        if (!hasTransitioned) // 檢查是否已經轉場
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
        hasTransitioned = true; // 標記轉場已開始

        jumpStartPosition = transform.position;

        float jumpDuration = jumpHeight / jumpSpeed;
        float elapsedTime = 0f;

        bool cloudEffectTriggered = false;

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

    // 可選：如果需要重置狀態（例如，用於測試目的）
    public void ResetTransitionState()
    {
        hasTransitioned = false;
        Debug.Log("轉場狀態已重置。");
    }
}