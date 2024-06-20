using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBySelf : MonoBehaviour
{
    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,1,0) * Time.deltaTime * rotateSpeed);

    }
}
