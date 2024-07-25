using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeAlertActivator : MonoBehaviour
{
	private CanvasGroup _canvasGroup;
	
	
	//Singleton
	public static GameObject ThisGameObject;
	private static EdgeAlertActivator instance;
	public static EdgeAlertActivator Instance {
		get {
			if(instance == null) {
				instance = ThisGameObject.GetComponent<EdgeAlertActivator>();
			}
			return instance;
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		ThisGameObject = gameObject;
		_canvasGroup = gameObject.GetComponent<CanvasGroup>();
		_canvasGroup.alpha = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ActivateEdgeAlert()
	{
		iTween.ValueTo(gameObject,iTween.Hash("From",0.0f,"To",1.0f,"Time",0.3f,"OnUpdateTarget",gameObject,"OnUpdate","FadeCallBack"));
	}

	public void DeactivateEdgeAlert()
	{
		iTween.ValueTo(gameObject,iTween.Hash("From",1.0f,"To",0.0f,"Time",0.3f,"OnUpdateTarget",gameObject,"OnUpdate","FadeCallBack"));
	}

	void FadeCallBack(float newValue)
	{
		_canvasGroup.alpha = newValue;
	}
}
