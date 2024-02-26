using UnityEngine;
using System.Collections; // Required for Coroutines

public class CrowController : MonoBehaviour
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
    private bool isDescending = false; // To check if the crow is in the descending phase

    void Start()
    {
        //MoveToNextPosition();
        hitParticles = GetComponentInChildren<ParticleSystem>();    }

    void Update()
    {
        if (isMoving && !isDescending)
        {
            MoveCrowHorizontally();
        }
        else if (isDescending)
        {
            DescendCrow();
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

        transform.position = endPosition; // Ensure the crow reaches the exact lift height
        isMoving = true; // Start moving horizontally after the crow has lifted and rotated
    }

    void MoveCrowHorizontally()
    {
        // Move towards the next position but maintain the current lifted Y position
        Vector3 horizontalTargetPosition = new Vector3(positions[currentPositionIndex].transform.position.x, transform.position.y, positions[currentPositionIndex].transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, horizontalTargetPosition, moveSpeed * Time.deltaTime);

        // Check if the crow is horizontally aligned with the next position
        if (Vector3.Distance(transform.position, horizontalTargetPosition) < 0.1f) // Use a small threshold to account for floating point imprecision
        {
            isDescending = true; // Start descending phase
        }
    }

    void DescendCrow()
    {
        // Define the target position with an additional 1 unit on the Y axis for hovering
        Vector3 hoverTargetPosition = new Vector3(positions[currentPositionIndex].transform.position.x, 
                                                positions[currentPositionIndex].transform.position.y + 0.65f, 
                                                positions[currentPositionIndex].transform.position.z);

        // Move towards the hover target position
        transform.position = Vector3.MoveTowards(transform.position, hoverTargetPosition, moveSpeed * Time.deltaTime);

        // Check if the crow has reached the hover target position (considering a small threshold for precision)
        if (Vector3.Distance(transform.position, hoverTargetPosition) < 0.1f)
        {
            isMoving = false;
            isDescending = false; // Reset for the next movement
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dodgeBallActive") && !isMoving && !isDescending)
        {
            PlayHitParticles(); // Play hit particles
            CheckNextPositionOrFlyAway();
        }
    }

    void PlayHitParticles()
    {
        if (hitParticles != null)
        {
            Debug.Log("Playing hit particles"); // Add this line for debugging
            hitParticles.Play(); // Play the particle system
        }
        else
        {
            Debug.LogError("Hit particles component not found!"); // This will help identify if the component is missing
        }
    }


    void CheckNextPositionOrFlyAway()
    {
        // Check if there are any positions left to land on
        if (currentPositionIndex < positions.Length - 1)
        {
            // If there are, move to the next position
            Debug.Log("Moving to next position.");
            MoveToNextPosition();
        }
        else
        {
            // If there are no positions left, fly away immediately
            Debug.Log("No more positions left. Flying away.");
            FlyAway();
        }
    }

    void MoveToNextPosition()
    {
        currentPositionIndex++;
        StartCoroutine(FlyUpAndRotate()); // Use coroutine to lift and rotate before moving horizontally
    }

    IEnumerator FlyAwaySequence()
    {
        // First, fly upwards to the lift height
        Vector3 startUpPosition = transform.position;
        Vector3 endUpPosition = new Vector3(transform.position.x, liftHeight, transform.position.z);
        float elapsedTimeUp = 0;

        while (elapsedTimeUp < liftDuration)
        {
            transform.position = Vector3.Lerp(startUpPosition, endUpPosition, elapsedTimeUp / liftDuration);
            elapsedTimeUp += Time.deltaTime;
            yield return null;
        }

        transform.position = endUpPosition; // Ensure the crow reaches the exact lift height

        // Wait for a brief moment at the top
        yield return new WaitForSeconds(1f); // Adjust the delay as needed

        // Then, move far away in the x-direction
        Vector3 startAwayPosition = transform.position;
        Vector3 endAwayPosition = new Vector3(transform.position.x + 100, transform.position.y, transform.position.z); // Change 100 to your desired distance
        float elapsedTimeAway = 0;
        float moveAwayDuration = 2f; // Adjust the duration for moving away as needed

        while (elapsedTimeAway < moveAwayDuration)
        {
            transform.position = Vector3.Lerp(startAwayPosition, endAwayPosition, elapsedTimeAway / moveAwayDuration);
            elapsedTimeAway += Time.deltaTime;
            yield return null;
        }

        // Optionally, show the win panel after the crow has moved away
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        // Finally, destroy or deactivate the crow as needed
        // Destroy(gameObject);
    }

    void FlyAway()
    {
        // Start the sequence
        StartCoroutine(FlyAwaySequence());
    }

}
