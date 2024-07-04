using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionController : MonoBehaviour
{
    public float jumpHeight; // XR原点跳起的高度
    public float transitionTime; // 过场效果的持续时间
    public int sceneToLoad;

    private void Start()
    {
        // 在开始时触发过场效果
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        // 将XR原点移动到目标位置
        Vector3 targetPosition = transform.position + Vector3.up * jumpHeight;
        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            // 使用插值函数平滑移动XR原点
            float t = elapsedTime / transitionTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 将屏幕渐变为黑色
        yield return new WaitForSeconds(1f); // 可以根据需要调整等待时间

        // 切换到下一个场景
        SceneManager.LoadScene(sceneToLoad); // 将"NextScene"替换为你要切换的场景名称
    }
}