using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionController2 : MonoBehaviour
{
    public float jumpHeight; // XR原点跳起的高度
    public float jumpSpeed; // XR原点跳起的速度
    public float transitionTime; // 过场效果的持续时间
    public int sceneToLoad;
    public SceneTransitionManager sceneTransitionManager; // Reference to the SceneTransitionManager
    public float timeToTriggerJump; // 触发跳跃之前的等待时间
    private Vector3 initialPosition; // 初始位置

    private void Start()
    {
        initialPosition = transform.position; // 记录初始位置
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        float jumpDuration = jumpHeight / jumpSpeed; // 计算跳起的持续时间
        float elapsedTime = 0f; // 过场效果计时器

        yield return new WaitForSeconds(timeToTriggerJump); // 等待触发跳跃的时间

        // 跳跃效果
        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float jumpAmount = Mathf.Lerp(0f, jumpHeight, t);
            transform.position = initialPosition + Vector3.up * jumpAmount;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Vector3 targetPosition = initialPosition + Vector3.up * jumpHeight;
        elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            float t = elapsedTime / transitionTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(transitionTime); // 等待过场效果的持续时间
        sceneTransitionManager.GoToSceneAsync(sceneToLoad);
    }
}