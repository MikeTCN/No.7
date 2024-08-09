using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionController3 : MonoBehaviour
{
    public float jumpHeight = 100f;
    public float jumpSpeed;
    public float transitionTime;
    public int sceneToLoad;
    public SceneTransitionManager sceneTransitionManager;

    private Vector3 jumpStartPosition;
    private bool isTransitionTriggered = false;
    private bool isAnimatorPaused = false;

    private MoveActivateAndPauseAnimator moveActivateScript;
    private Animator objectAnimator;

    private void Start()
    {
        moveActivateScript = GetComponent<MoveActivateAndPauseAnimator>();
        if (moveActivateScript == null)
        {
            Debug.LogError("MoveActivateAndPauseAnimator script not found on this GameObject!");
        }
        else
        {
            objectAnimator = moveActivateScript.objectToMove.GetComponent<Animator>();
            if (objectAnimator == null)
            {
                Debug.LogError("Animator not found on the object to move!");
            }
            else
            {
                StartCoroutine(CheckAnimatorState());
            }
        }
    }

    private IEnumerator CheckAnimatorState()
    {
        while (!isTransitionTriggered)
        {
            if (objectAnimator.speed == 0)
            {
                isAnimatorPaused = true;
            }
            else
            {
                isAnimatorPaused = false;
            }
            yield return new WaitForSeconds(0.1f); // 每0.1秒檢查一次
        }
    }

    public void StartTransition()
    {
        if (!isTransitionTriggered && isAnimatorPaused)
        {
            isTransitionTriggered = true;
            StartCoroutine(TransitionCoroutine());
        }
        else if (!isAnimatorPaused)
        {
            Debug.LogWarning("Cannot start transition: Animator is not paused.");
        }
    }

    private IEnumerator TransitionCoroutine()
    {
        jumpStartPosition = transform.position;

        float jumpDuration = jumpHeight / jumpSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float jumpAmount = Mathf.Lerp(0f, jumpHeight, t);
            Vector3 newPosition = jumpStartPosition + Vector3.up * jumpAmount;
            transform.position = newPosition;

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
}