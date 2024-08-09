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

    public void StartTransition()
    {
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
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
}