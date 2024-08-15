using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class ObjPositionAutoReset : MonoBehaviour
{
    [SerializeField] AudioClip resetSound;
    private Vector3 origPosition;
    private Quaternion origRotation;
    public GameObject targetTransform;
    private float timer = 0;
    public bool needReset = false;
    [SerializeField] private float timeThreshold = 3f;

    void Start()
    {
        origPosition = this.transform.position;
        origRotation = this.transform.rotation;
        timer = 0;

        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.AddListener(GrabCheck);
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectExited.AddListener(ReleaseCheck);
    }

    void Update()
    {
        //Debug.Log(timer);
        if (needReset && !GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().isSelected && transform.position!=origPosition && transform.rotation!=origRotation)
        {
            //Debug.Log("Object position changed");
            timer += Time.deltaTime;
            if (timer > timeThreshold)
            {
                Debug.Log("Resetting object position");
                transform.position = origPosition;
                transform.rotation = origRotation;
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().movementType = UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable.MovementType.VelocityTracking;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                
                //create a audiosource if there is none
                if (GetComponent<AudioSource>() == null)
                {
                    gameObject.AddComponent<AudioSource>();
                }
                GetComponent<AudioSource>().playOnAwake = false;
                GetComponent<AudioSource>().clip = resetSound;
                GetComponent<AudioSource>().loop = false;
                GetComponent<AudioSource>().volume = 0.8f;
                GetComponent<AudioSource>().spatialize = true;
                GetComponent<AudioSource>().spatializePostEffects = true;
                GetComponent<AudioSource>().outputAudioMixerGroup = targetTransform.GetComponent<AudioSource>().outputAudioMixerGroup;
                GetComponent<AudioSource>().Play();
                timer = 0;
                //needReset = false; // ���ú�needReset����Ϊfalse
            }
        }
    }

    private void GrabCheck(SelectEnterEventArgs args)
    {
        timer = 0;
        needReset = false; // ������ץȡʱ������Ҫ����
    }

    private void ReleaseCheck(SelectExitEventArgs args)
    {
        needReset = true; // �������ͷ�ʱ����ʼ��ʱ����
    }

    private void OnDisable()
    {
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.RemoveListener(GrabCheck);
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectExited.RemoveListener(ReleaseCheck);
    }

    public void SetTarget(GameObject target)
    {
        targetTransform = target;
        origPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        origRotation = new Quaternion(target.transform.rotation.x, target.transform.rotation.y, target.transform.rotation.z, target.transform.rotation.w);
    }
}
