using UnityEngine;

public class LandingPos : MonoBehaviour
{
    /*public GameObject landingPoint;

    public GameObject playerModel;

    public float explosionForce = 10f; // Force of the explosion
    public float gravity = 9.81f; // Acceleration due to gravity

    void Update()
    {
        CalculateLandingPosition();
    }

    void CalculateLandingPosition()
    {
        Vector3 initialPosition = transform.position;
        Vector3 initialVelocity = playerModel.transform.forward * explosionForce;

        // Calculate the time it takes for the bullet to reach the ground
        float timeToGround = Mathf.Sqrt(2 * initialPosition.y / gravity);

        // Predict the landing position
        Vector3 landingPosition = initialPosition + initialVelocity * timeToGround;

        // Position the landing point GameObject
        landingPoint.transform.position = landingPosition;

        // Rotate the landing point based on the player's rotation
        // Assuming the player's rotation is set elsewhere in the code
        landingPoint.transform.rotation = Quaternion.identity; // Modify as needed


        // Debug draw the landing position (optional)
        Debug.DrawLine(initialPosition, landingPosition, Color.green);
    }*/
}
