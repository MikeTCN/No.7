using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionController2 : MonoBehaviour
{
    public float jumpHeight; // XR原點跳起的高度
    public float jumpSpeed; // XR原點跳起的速度
    public float transitionTime; // 過場效果的持續時間
    public int sceneToLoad;
    public SceneTransitionManager sceneTransitionManager; // Reference to the SceneTransitionManager
    public float timeToTriggerJump; // 觸發跳躍之前的等待時間
    private Vector3 jumpStartPosition; // 跳躍開始的位置

    private void Start()
    {
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        yield return new WaitForSeconds(timeToTriggerJump - 0.5f); // 等待觸發跳躍的時間減去0.5秒

        // 記錄跳躍前0.5秒的位置
        jumpStartPosition = transform.position;

        yield return new WaitForSeconds(0.5f); // 等待0.5秒

        float jumpDuration = jumpHeight / jumpSpeed; // 計算跳起的持續時間
        float elapsedTime = 0f; // 過場效果計時器

        // 跳躍效果
        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float jumpAmount = Mathf.Lerp(0f, jumpHeight, t);
            transform.position = jumpStartPosition + Vector3.up * jumpAmount;

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

        yield return new WaitForSeconds(transitionTime); // 等待過場效果的持續時間
        sceneTransitionManager.GoToSceneAsync(sceneToLoad);
    }
}