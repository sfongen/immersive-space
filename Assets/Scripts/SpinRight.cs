using UnityEngine;

public class SpinRight : MonoBehaviour
{
    public float speed = 50f; // Rotation speed in degrees per second

    void Update()
    {
        // Rotate around the object's local right axis (X-axis)
        transform.Rotate(Vector3.right, speed * Time.deltaTime, Space.Self);
    }
}
