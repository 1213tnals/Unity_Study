using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarVisualization : MonoBehaviour
{
    public int numberOfRays = 360; // Number of rays to be cast
    public float maxDistance = 10f; // Max distance of the rays
    public float rotationSpeed = 10f; // Speed of the sensor rotation

    void Update()
    {
        // Rotate the sensor
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Cast rays in a circle
        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfRays;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red); // Draw ray in editor for visualization
                // You can process hit information here
                Debug.Log("Hit: " + hit.collider.name + " at distance: " + hit.distance);
            }
            else
            {
                Debug.DrawRay(ray.origin, direction * maxDistance, Color.green); // Draw ray in editor for visualization
            }
        }
    }
}
