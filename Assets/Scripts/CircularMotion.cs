using UnityEngine;
using UnityEngine.Serialization;

public class CircularMotion : MonoBehaviour
{
    public float radius = 1.0f; // Radius of the circle
    public float speed = 1.0f; // Speed of rotation
    
    public float angle = 0.0f; // Current angle

    void Update()
    {
        // Calculate the new position based on the current angle
        Vector3 newPosition = new Vector3(
            radius * Mathf.Cos(angle),
            0.0f,
            radius * Mathf.Sin(angle)
        );

        // Update the object's position
        transform.position = newPosition;

        // Increase the angle by the speed for the next frame
        angle += speed * Time.deltaTime;
    }
}