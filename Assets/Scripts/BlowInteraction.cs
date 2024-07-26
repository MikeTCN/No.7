using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class BlowInteraction : MonoBehaviour
{
    public XRController controller;
    public InputActionReference blowAction;
    public Transform blowZone;
    public float blowZoneRadius = 0.2f;
    public Animator targetAnimator;
    public string animationTriggerName = "BlowTrigger";

    private bool isInBlowZone = false;

    private void OnEnable()
    {
        blowAction.action.performed += OnBlow;
    }

    private void OnDisable()
    {
        blowAction.action.performed -= OnBlow;
    }

    private void Update()
    {
        // 檢查手是否在吹氣區域內
        if (Vector3.Distance(controller.transform.position, blowZone.position) <= blowZoneRadius)
        {
            isInBlowZone = true;
        }
        else
        {
            isInBlowZone = false;
        }
    }

    private void OnBlow(InputAction.CallbackContext context)
    {
        if (isInBlowZone)
        {
            // 觸發動畫
            targetAnimator.SetTrigger(animationTriggerName);
            Debug.Log("吹氣動作觸發！");
        }
    }

    // 可視化吹氣區域（僅在編輯器中）
    private void OnDrawGizmos()
    {
        if (blowZone != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(blowZone.position, blowZoneRadius);
        }
    }
}