using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;


public class SphereBehavior : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public float flySpeed = 10f; // �����ٶ�
    public float flyTime = 2f; // ����ʱ��

    private bool wasThrown = false; // ����Ƿ񱻶���

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease); // ���������¼�
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // ��������Ǳ��������Ƿ���
        //if (args.interactorObject is XRDirectInteractor)
        {
            Debug.Log("123");
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null )
            {
                wasThrown = true;
                StartCoroutine(FlyAndDisappear(rb.velocity));
            }
        }
    }

    IEnumerator FlyAndDisappear(Vector3 initialVelocity)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float elapsedTime = 0f;

        while (elapsedTime < flyTime)
        {
            rb.velocity = initialVelocity;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); // ɾ��Sphere����
    }
}
