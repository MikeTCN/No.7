using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour {
	Collider[] currentColliders;
	Camera currentCamera;
	float distanceToCamera;

	// Use this for initialization
	void Start () {
		currentColliders = gameObject.GetComponentsInChildren<Collider> (true);
		currentCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentCamera && currentColliders.Length > 0) {
			distanceToCamera = Vector3.Distance (gameObject.transform.position, currentCamera.transform.position);

			if (distanceToCamera < 10.0f) {
				EnableOrDisableColliders (true);
			} else {
				EnableOrDisableColliders (false);
			}
		}
	}

	void EnableOrDisableColliders(bool isActive){
		for (int i = 0; i < currentColliders.Length; ++i) {
			currentColliders [i].enabled = isActive;
		}
	}
}



