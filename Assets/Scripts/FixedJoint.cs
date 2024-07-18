using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FixedJoint : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private ConfigurableJoint joint;

    public UnityEvent onStartGame;

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<ConfigurableJoint>();

        // 禁用重力
        rb.useGravity = false;

        // 將ConfigurableJoint的connectedBody設置為null
        joint.connectedBody = null;

        // 設置抓取開始事件
        grabInteractable.selectEntered.AddListener(StartGame);
    }

    private void StartGame(SelectEnterEventArgs args)
    {
        // 啟用重力
        rb.useGravity = true;

        // 將ConfigurableJoint的connectedBody設置為null
        joint.connectedBody = null;

        // 調用開始遊戲事件
        onStartGame.Invoke();
    }
}