using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabTrigger : MonoBehaviour
{
    public TeleportToSphere teleportScript;
    public PlayableDirector timeline3;  // 添加Timeline3的引用
    private XRGrabInteractable grabInteractable;

    private void Start()
    {
        // 获取XRGrabInteractable组件
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogError("GrabTrigger脚本需要XRGrabInteractable组件！");
            return;
        }

        // 监听抓取事件
        grabInteractable.selectEntered.AddListener(OnGrab);

        if (teleportScript == null)
        {
            Debug.LogWarning("TeleportToSphere脚本未分配！");
        }

        if (timeline3 == null)
        {
            Debug.LogWarning("Timeline3未分配！");
        }
    }

    // 当物体被抓住时调用
    public void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("物体被抓住了！");

        if (teleportScript != null)
        {
            // 启动TeleportToSphere脚本
            teleportScript.enabled = true;
            Debug.Log("已启动TeleportToSphere脚本");
        }
        else
        {
            Debug.LogError("TeleportToSphere脚本未找到，无法启动！");
        }

        if (timeline3 != null)
        {
            // 播放Timeline3
            timeline3.Play();
            Debug.Log("已播放Timeline3");
        }
        else
        {
            Debug.LogError("Timeline3未找到，无法播放！");
        }
    }

    private void OnDisable()
    {
        // 移除事件监听，防止内存泄漏
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }
}