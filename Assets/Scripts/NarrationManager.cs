using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NarrationManager : MonoBehaviour
{
    public AudioSource narrationAudio; // 旁白音频
    public XRGrabInteractable[] spheres; // 所有Sphere的XRGrabInteractable引用
    public SphereBehavior[] sphereBehaviors;

    void Start()
    {
        StartCoroutine(WaitForNarrationToEnd());
    }

    // 等待旁白音频播放结束的协程
    IEnumerator WaitForNarrationToEnd()
    {
        yield return new WaitForSeconds(narrationAudio.clip.length); // 等待旁白音频播放完毕
        EnableSphereInteraction(); // 启用Sphere的XR交互功能
    }

    // 启用Sphere的XR交互功能
    void EnableSphereInteraction()
    {
        foreach (var sphere in spheres)
        {
            sphere.enabled = true; // 启用XRGrabInteractable组件
        }
        foreach (var sphere in sphereBehaviors)
        {
            sphere.enabled = true; // 启用XRGrabInteractable组件
        }
    }
}