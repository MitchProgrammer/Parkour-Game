using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float minYLevel = -48;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < minYLevel)
        {
            transform.position += 2 * Mathf.Abs(minYLevel) * Vector3.up;
        }
    }
}
