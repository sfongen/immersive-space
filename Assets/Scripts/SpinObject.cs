using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public float speed = 50f; // Rotation speed in degrees per second
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial rotation of the object
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Calculate the custom rotation axis based on the initial tilt
        Vector3 rotationAxis = initialRotation * Vector3.up;

        // Rotate around the custom axis
        transform.Rotate(rotationAxis, speed * Time.deltaTime, Space.World);
    }
}
