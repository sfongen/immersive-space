using UnityEngine;

public class SpinForward : MonoBehaviour
{
    public float speed = 50f; // Rotation speed in degrees per second

    void Update()
    {
        // Rotate around the object's local forward axis (Z-axis)
        transform.Rotate(Vector3.forward, speed * Time.deltaTime, Space.Self);
    }
}
