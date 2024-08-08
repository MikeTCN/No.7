using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionController2 : MonoBehaviour
{
    public float jumpHeight = 100f; // 增加跳躍高度以確保穿越雲層
    public float jumpSpeed;
    public float transitionTime;
    public int sceneToLoad;
    public SceneTransitionManager sceneTransitionManager;
    public float timeToTriggerJump;
    private Vector3 jumpStartPosition;

    public ParticleSystem cloudEffect; // 雲效果的粒子系統
    public float cloudEffectHeight = 80f; // 觸發雲效果的高度

    private void Start()
    {
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        yield return new WaitForSeconds(timeToTriggerJump - 0.5f);

        jumpStartPosition = transform.position;

        yield return new WaitForSeconds(0.5f);

        float jumpDuration = jumpHeight / jumpSpeed;
        float elapsedTime = 0f;

        bool cloudEffectTriggered = false;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float jumpAmount = Mathf.Lerp(0f, jumpHeight, t);
            Vector3 newPosition = jumpStartPosition + Vector3.up * jumpAmount;
            transform.position = newPosition;

            // 檢查是否達到雲效果高度
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
}