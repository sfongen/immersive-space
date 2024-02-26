using UnityEngine;
using System.Collections; // Required for Coroutines

public class CrowController : MonoBehaviour
{
    public GameObject[] positions;
    private int currentPositionIndex = 0;
    public float moveSpeed = 5f;
    private bool isMoving = false;
    public GameObject winPanel;
    public float liftHeight = 13f; // Adjust as necessary
    public float liftDuration = 1f; // Duration for lifting up animation
    public float rotationSpeed = 2f; // Adjust as necessary
    private bool isDescending = false; // To check if the crow is in the descending phase

    void Start()
    {
        MoveToNextPosition();
    }

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

            // If it's the last position, make the crow fly away
            if (currentPositionIndex == positions.Length - 1)
            {
                FlyAway();
            }
        }
    }


    void MoveToNextPosition()
    {
        if (currentPositionIndex < positions.Length - 1)
        {
            currentPositionIndex++;
            StartCoroutine(FlyUpAndRotate()); // Use coroutine to lift and rotate before moving horizontally
        }
    }

    private void OnTriggerEnter(Collider other)
{
    Debug.Log($"OnTriggerEnter called. isMoving: {isMoving}, isDescending: {isDescending}, Tag: {other.tag}");

    if (other.CompareTag("dodgeBallActive") && !isMoving && !isDescending)
    {
        Debug.Log("Moving to next position.");
        MoveToNextPosition();
    }
}

    void FlyAway()
    {
        winPanel.SetActive(true);
        Destroy(gameObject); // Or move it far away
    }
}
