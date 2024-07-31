using System.Collections;
using UnityEngine;

public class MoveAndActivateAnimator : MonoBehaviour
{
    public ParticleSystem particles; // Reference to the ParticleSystem
    public GameObject objectToMove; // Reference to the GameObject to move
    public Vector3 targetPosition = new Vector3(230.789993f, 38.5009995f, 272.664001f); // Target position to move the object
    private bool hasReachedTarget = false; // Flag to track if the object has reached the target position

    void Update()
    {
        if (particles.isPlaying && !hasReachedTarget) // Check if the particle system is playing and the object has not reached the target position
        {
            MoveObjectToTarget();
        }
    }

    void MoveObjectToTarget()
    {
        objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, targetPosition, Time.deltaTime); // Move the object towards the target position

        if (objectToMove.transform.position == targetPosition) // Check if the object has reached the target position
        {
            ActivateAnimator();
            hasReachedTarget = true; // Set the flag to true once the object reaches the target position
        }
    }

    void ActivateAnimator()
    {
        Animator objectAnimator = objectToMove.GetComponent<Animator>(); // Get the Animator component of the object
        if (objectAnimator != null)
        {
            objectAnimator.enabled = true; // Enable the Animator component
        }
    }
}