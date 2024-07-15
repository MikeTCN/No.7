using UnityEngine;
using System.Collections;

public class FlyIntoSkyNoTransition : MonoBehaviour
{
    public float jumpHeight; // XR原点跳起的高度
    public float jumpSpeed; // XR原点跳起的速度
    public float transitionTime; // 过场效果的持续时间

    private Vector3 initialPosition; // 初始位置

    private void Start()
    {
        initialPosition = transform.position; // 记录初始位置
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        // 计算跳起的持续时间
        float jumpDuration = jumpHeight / jumpSpeed;

        // XR原点跳起
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            // 使用插值函数将XR原点跳起
            float t = elapsedTime / jumpDuration;
            float jumpAmount = Mathf.Lerp(0f, jumpHeight, t);
            transform.position = initialPosition + Vector3.up * jumpAmount;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 将XR原点移动到目标位置
        Vector3 targetPosition = initialPosition + Vector3.up * jumpHeight;
        elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            // 使用插值函数平滑移动XR原点
            float t = elapsedTime / transitionTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}