using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFinder : MonoBehaviour
{
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    public float checkDistance;

    private Vector3 normal = Vector3.up;

    public Rigidbody rb;
    public float jumpForce = 5f;

    public PlayerIdentity PI;

    public void Update()
    {
        FindFloor();  
    }

    public void FindFloor()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, playerHeight * 0.5f + checkDistance, whatIsGround);

        if (grounded) normal = hit.normal;
        else normal = Vector3.up;

        if (normal == Vector3.zero)
        {
            normal = Vector3.up;
        }


        if (PI.player == PlayerIdentity.Players.player1)
        {
            if (Input.GetKey(KeyCode.E) && grounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        else if (PI.player == PlayerIdentity.Players.player2)
        {
            if (Input.GetKey(KeyCode.RightControl) && grounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        return;
    }

    public Vector3 Normal => normal;
}
