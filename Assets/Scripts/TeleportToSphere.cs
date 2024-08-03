using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class TeleportToSphere : MonoBehaviour
{
    public float delayBeforeFlight = 3f;
    public float flightDuration = 2f;
    public float followDuration = 3f; // 跟随目标的时间
    public GameObject targetSphere;
    public int sceneToLoad;
    public XROrigin xrOrigin;
    public float collisionCheckRadius = 1f; // 增加碰撞检查半径
    public float maxDistanceFromTarget = 2f; // 最大允许距离

    private bool hasFlown = false;
    public SceneTransitionManager sceneTransitionManager;

    private void Start()
    {
        Debug.Log("TeleportToSphere脚本已启动");
        if (xrOrigin == null)
        {
            xrOrigin = GetComponent<XROrigin>();
            if (xrOrigin == null)
            {
                Debug.LogError("未找到 XR Origin 组件！");
                return;
            }
        }
        StartCoroutine(FlightCoroutine());
    }

    private IEnumerator FlightCoroutine()
    {
        Debug.Log("开始等待飞行...");
        yield return new WaitForSeconds(delayBeforeFlight);

        if (targetSphere != null && xrOrigin != null)
        {
            Debug.Log("开始飞行到目标位置...");
            Vector3 startPosition = xrOrigin.transform.position;
            Vector3 offset = xrOrigin.transform.position - xrOrigin.Camera.transform.position;
            offset.y = 0; // 保持y轴偏移为0，防止穿透地面

            float elapsedTime = 0f;
            while (elapsedTime < flightDuration)
            {
                Vector3 targetPosition = targetSphere.transform.position + offset;
                xrOrigin.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / flightDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            hasFlown = true;
            Debug.Log("飞行完成。开始跟随目标。");

            // 飞行完成后，继续跟随目标一段时间
            float followTime = 0f;
            while (followTime < followDuration)
            {
                Vector3 targetPosition = targetSphere.transform.position + offset;
                xrOrigin.transform.position = Vector3.MoveTowards(xrOrigin.transform.position, targetPosition, Time.deltaTime * 5f);

                if (CheckCollisionWithTarget())
                {
                    Debug.Log("在跟随过程中检测到碰撞！");
                    StartCoroutine(TransitionToNextScene());
                    yield break;
                }

                followTime += Time.deltaTime;
                yield return null;
            }

            Debug.Log("跟随完成。继续检查碰撞。");

            // 跟随结束后，继续检查一段时间的碰撞
            float checkTime = 0f;
            while (checkTime < 2f) // 持续检查2秒
            {
                if (CheckCollisionWithTarget())
                {
                    Debug.Log("在额外检查时间内检测到碰撞！");
                    StartCoroutine(TransitionToNextScene());
                    yield break;
                }

                checkTime += Time.deltaTime;
                yield return null;
            }

            Debug.Log("未能在指定时间内触发碰撞。");
        }
        else
        {
            Debug.LogError("目标球体或 XR Origin 未设置！");
        }
    }

    private bool CheckCollisionWithTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(xrOrigin.Camera.transform.position, collisionCheckRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == targetSphere)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("触发了碰撞。碰撞对象: " + other.gameObject.name);
        if (hasFlown && other.gameObject == targetSphere)
        {
            Debug.Log("触发场景转换...");
            StartCoroutine(TransitionToNextScene());
        }
    }

    private IEnumerator TransitionToNextScene()
    {
        if (sceneTransitionManager != null)
        {
            Debug.Log("使用 SceneTransitionManager 加载场景: " + sceneToLoad);
            sceneTransitionManager.GoToSceneAsync(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("SceneTransitionManager 未分配。直接加载场景。");
            SceneManager.LoadScene(sceneToLoad);
        }

        yield return null;
    }

    private void OnDrawGizmos()
    {
        if (xrOrigin != null && xrOrigin.Camera != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(xrOrigin.Camera.transform.position, collisionCheckRadius);
        }
    }
}