using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectGrabController : MonoBehaviour
{
    public XRGrabInteractable[] objectsToMonitor; // 需要监测是否被抓握的物体
    public GameObject[] objectsToAppear; // 要出现的物体
    public Vector3[] targetScales; // 每个物体的目标缩放比例
    public float scaleSpeed = 1f; // 放大速度

    private void Start()
    {
        // 初始化时，将对象缩小到不可见状态
        for (int i = 0; i < objectsToAppear.Length; i++)
        {
            objectsToAppear[i].transform.localScale = Vector3.zero;
            objectsToAppear[i].SetActive(false); // 隐藏对象
        }

        // 为每个需要监测的物体添加抓取事件监听器
        foreach (XRGrabInteractable grabInteractable in objectsToMonitor)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
        }
    }

    // 当物体被抓住时调用
    private void OnGrab(SelectEnterEventArgs args)
    {
        StartCoroutine(ShowAndScaleObjects());
    }

    // 放大物体的协程
    private IEnumerator ShowAndScaleObjects()
    {
        for (int i = 0; i < objectsToAppear.Length; i++)
        {
            GameObject obj = objectsToAppear[i];
            obj.SetActive(true); // 显示对象
            Vector3 initialScale = Vector3.zero;
            Vector3 targetScale = targetScales[i]; // 使用设置的目标缩放比例
            float progress = 0f;

            while (progress < 1f)
            {
                progress += Time.deltaTime * scaleSpeed;
                obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, progress);
                yield return null;
            }

            obj.transform.localScale = targetScale; // 确保最终达到目标大小
        }
    }
}