using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabTrigger : MonoBehaviour
{
    public TeleportToSphere teleportScript;
    private XRGrabInteractable grabInteractable;

    private void Start()
    {
        // 獲取XRGrabInteractable組件
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogError("GrabTrigger腳本需要XRGrabInteractable組件！");
            return;
        }

        // 監聽抓取事件
        grabInteractable.selectEntered.AddListener(OnGrab);

        if (teleportScript == null)
        {
            Debug.LogWarning("TeleportToSphere腳本未分配！");
        }
    }

    // 當物體被抓住時調用
    public void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("物體被抓住了！");
        if (teleportScript != null)
        {
            // 啟動TeleportToSphere腳本
            teleportScript.enabled = true;
            Debug.Log("已啟動TeleportToSphere腳本");
        }
        else
        {
            Debug.LogError("TeleportToSphere腳本未找到，無法啟動！");
        }
    }

    private void OnDisable()
    {
        // 移除事件監聽，防止內存洩漏
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
        }
    }
}