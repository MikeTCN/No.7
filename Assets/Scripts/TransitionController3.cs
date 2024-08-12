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
    private bool hasTransitioned = false; // 新增：用于标记是否已经转场

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
            yield return new WaitForSeconds(0.1f); // 每0.1秒检查一次
        }
    }

    public void StartTransition()
    {
        if (!hasTransitioned && isAnimatorPaused) // 修改：检查是否已经转场
        {
            isTransitionTriggered = true;
            hasTransitioned = true; // 标记已经转场
            StartCoroutine(TransitionCoroutine());
        }
        else if (hasTransitioned)
        {
            Debug.LogWarning("转场已经触发过，忽略此次请求。");
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

    // 可选：如果需要重置状态（例如，用于测试目的）
    public void ResetTransitionState()
    {
        hasTransitioned = false;
        isTransitionTriggered = false;
        Debug.Log("转场状态已重置。");
    }
}