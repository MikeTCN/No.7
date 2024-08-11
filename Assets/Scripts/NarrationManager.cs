using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NarrationManager : MonoBehaviour
{
    public AudioSource narrationAudio; // �԰���Ƶ
    public XRGrabInteractable[] spheres; // ����Sphere��XRGrabInteractable����
    public SphereBehavior[] sphereBehaviors;

    void Start()
    {
        StartCoroutine(WaitForNarrationToEnd());
    }

    // �ȴ��԰���Ƶ���Ž�����Э��
    IEnumerator WaitForNarrationToEnd()
    {
        yield return new WaitForSeconds(narrationAudio.clip.length); // �ȴ��԰���Ƶ�������
        EnableSphereInteraction(); // ����Sphere��XR��������
    }

    // ����Sphere��XR��������
    void EnableSphereInteraction()
    {
        foreach (var sphere in spheres)
        {
            sphere.enabled = true; // ����XRGrabInteractable���
        }
        foreach (var sphere in sphereBehaviors)
        {
            sphere.enabled = true; // ����XRGrabInteractable���
        }
    }
}