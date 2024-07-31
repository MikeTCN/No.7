using System.Collections;
using UnityEngine;

public class MoveActivateAndPauseAnimator : MonoBehaviour
{
    public ParticleSystem particles; // Reference to the ParticleSystem
    public GameObject objectToMove; // Reference to the GameObject to move
    public Vector3 targetPosition = new Vector3(230.789993f, 38.5009995f, 272.664001f); // Target position to move the object
    private bool hasReachedTarget = false; // Flag to track if the object has reached the target position
    private Animator objectAnimator; // Reference to the Animator component of the object
    private float pauseDuration = 60f; // Duration to pause the animator

    void Start()
    {
        objectAnimator = objectToMove.GetComponent<Animator>(); // Get the Animator component of the object
    }

    void Update()
    {
        if (particles.isPlaying && !hasReachedTarget) // Check if the particle system is playing and the object has not reached the target position
        {
            MoveObjectToTarget();
        }
        else if (hasReachedTarget) // If the object has reached the target position
        {
            PauseAnimator();
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
        if (objectAnimator != null)
        {
            objectAnimator.enabled = true; // Enable the Animator component
        }
    }

    void PauseAnimator()
    {
        if (objectAnimator != null)
        {
            pauseDuration -= Time.deltaTime; // Decrease the pause duration
            if (pauseDuration <= 0)
            {
                objectAnimator.speed = 0; // Pause the animator by setting speed to 0
            }
        }
    }
}