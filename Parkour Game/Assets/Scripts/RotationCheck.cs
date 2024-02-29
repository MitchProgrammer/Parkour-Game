using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCheck : MonoBehaviour
{
    public Rigidbody rb;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Get the current local rotation
        Quaternion currentRotation = transform.localRotation;

        // Modify only the X and Z Euler angles
        Vector3 newEulerAngles = new Vector3(0f, currentRotation.eulerAngles.y, 0f);

        // Set the local rotation with modified Euler angles
        transform.localRotation = Quaternion.Euler(newEulerAngles);
    }
}
