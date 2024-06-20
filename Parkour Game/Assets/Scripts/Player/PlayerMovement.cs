using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //[Header("HeaderName")]

    // Player Movement
    public float movementSpeed;
    public float rotationSpeed;

    public float movementInputX;
    public float movementInputZ;

    // Rigidbogy
    public Rigidbody rb;

    // Animator
    public Animator anim;

    // Players
    [HideInInspector]
    public GameObject player1;
    [HideInInspector]
    public GameObject player2;

    // Player Identification
    public PlayerIdentity PI;

    // Start is called before the first frame updates
    public void Start()
    {
        anim = GetComponentInChildren<Animator>();

        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        Debug.Log("PlayerMovement.cs loaded successfully");
    }

    public void FixedUpdate()
    {
        if (PI.player == PlayerIdentity.Players.player1)
        {
            movementInputX = Input.GetAxis("Horizontal") * rotationSpeed;
            movementInputZ = Input.GetAxis("Vertical");
            
        }
        else
        {
            movementInputX = Input.GetAxis("Horizontal2") * rotationSpeed;
            movementInputZ = Input.GetAxis("Vertical2");
        }

        if (movementInputX != 0 || movementInputZ != 0)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        // if the key is pressed
        MovePlayer(movementInputX, movementInputZ);
        RotatePlayer();

        //anim.SetBool("IsMoving", false);
        /*
        Vector3 cameraX = Camera.main.transform.right;
        Vector3 cameraZ = Vector3.Cross(cameraX, Vector3.up);

        inputDirection = camerX * x + cameraZ * y;
        inputDirection.Normalise();
        */
    }

    public void MovePlayer(float movementInputX, float movementInputZ)
    {
        anim.SetFloat("XInput", movementInputX);

        // Calculate movement direction based on world axes
        Vector3 movementDirection = new Vector3(movementInputX, 0f, movementInputZ).normalized;

        // Move the player in the direction of the calculated movement direction
        rb.MovePosition(transform.position + movementDirection * movementSpeed * Time.deltaTime);

        //anim.SetFloat("XInput", animVel)
        // transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    public void RotatePlayer()
    {
        if (PI.player == PlayerIdentity.Players.player1)
        {
            Vector3 targetDirection = player2.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f);
        }

        if (PI.player == PlayerIdentity.Players.player2)
        {
            Vector3 targetDirection = player1.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f);
        }

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);


        // Quaternion target = Quaternion.Euler(0f, rotation, 0f);
        // transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        //transform.Rotate(0f, rotationInput, 0f);

        /*
        Quaternion targetDir = Quaternion.LookRotation(inputDirection);
        Quaternion lerpedDir = Quaternion.Lerp(transform.rotation, targetDir, turnSpeed * Time.deltaTime);
        transform.rotation = lerpedDir;
        */
    }
}
