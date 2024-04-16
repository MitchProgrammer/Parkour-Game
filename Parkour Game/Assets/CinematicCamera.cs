using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CinematicCamera : MonoBehaviour
{
    // Rotation speed
    public float smoothness = 2.0f;
    public float previousTime;

    public Vector3 rotation;

    public void Update()
    {
        if ((Time.time - previousTime) > 0.5)
        {
            rotation = Random.insideUnitSphere;

            previousTime = Time.time;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), smoothness * Time.deltaTime);
    }
}
