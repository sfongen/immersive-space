using UnityEngine;
using System.Collections; // Required for Coroutines

public class BeeController : MonoBehaviour
{
    public GameObject[] positions;
    private int currentPositionIndex = 0;
    public float moveSpeed = 5f;
    private bool isMoving = false;
    public GameObject winPanel;
    public ParticleSystem hitParticles; // Reference to the ParticleSystem component
    public float liftHeight = 13f; // Adjust as necessary
    public float liftDuration = 1f; // Duration for lifting up animation
    public float rotationSpeed = 2f; // Adjust as necessary
    private bool isDescending = false; // To check if the bee is in the descending phase

    void Start()
    {
        hitParticles = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (isMoving && !isDescending)
        {
            MoveBeeHorizontally();
        }
        else if (isDescending)
        {
            DescendBee();
        }
    }

    IEnumerator FlyUpAndRotate()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x, liftHeight, transform.position.z);
        Vector3 directionToNextPosition = (positions[currentPositionIndex].transform.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(directionToNextPosition);
        float elapsedTime = 0;

        while (elapsedTime < liftDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / liftDuration);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, elapsedTime / liftDuration * rotationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition; // Ensure the bee reaches the exact lift height
        isMoving = true; // Start moving horizontally after the bee has lifted and rotated
    }

    void MoveBeeHorizontally()
    {
        Vector3 horizontalTargetPosition = new Vector3(positions[currentPositionIndex].transform.position.x, transform.position.y, positions[currentPositionIndex].transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, horizontalTargetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, horizontalTargetPosition) < 0.1f)
        {
            isDescending = true;
        }
    }

    void DescendBee()
    {
        Vector3 hoverTargetPosition = new Vector3(positions[currentPositionIndex].transform.position.x, 
                                                   positions[currentPositionIndex].transform.position.y + 0.65f, 
                                                   positions[currentPositionIndex].transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, hoverTargetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, hoverTargetPosition) < 0.1f)
        {
            isMoving = false;
            isDescending = false; // Reset for the next movement
            // No immediate action on descent completion to wait for the next ball hit
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dodgeBallActive") && !isMoving && !isDescending)
        {
            PlayHitParticles(); // Play hit particles
            CheckNextPositionOrLoopBack(); // Move to the next position or loop back
        }
    }

    void PlayHitParticles()
    {
        if (hitParticles != null)
        {
            hitParticles.Play();
        }
        else
        {
            Debug.LogError("Hit particles component not found!");
        }
    }

    void CheckNextPositionOrLoopBack()
    {
        if (currentPositionIndex < positions.Length - 1)
        {
            currentPositionIndex++; // Move to the next position
        }
        else
        {
            currentPositionIndex = 0; // Loop back to the first position if it's the last one
        }
        StartCoroutine(FlyUpAndRotate()); // Starts the process to move to the next or first position
    }
}
