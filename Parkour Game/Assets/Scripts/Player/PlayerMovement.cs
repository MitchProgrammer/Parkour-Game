using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //[Header("HeaderName")]

    // Player Movement
    public float movementSpeed;
    public float rotationSpeed;

    public float rotationInput;
    public float movementInput;

    // Rigidbogy
    public Rigidbody rb;

    // Animator
    public Animator anim;

    // Player Identification
    public PlayerIdentity PI;

    // Start is called before the first frame updates
    public void Start()
    {
        anim = GetComponentInChildren<Animator>();

        Debug.Log("PlayerMovement.cs loaded successfully");
    }

    public void FixedUpdate()
    {
        if (PI.player == PlayerIdentity.Players.player1)
        {
            rotationInput = Input.GetAxis("Horizontal") * rotationSpeed;
            movementInput = Input.GetAxis("Vertical");
            
        }
        else
        {
            rotationInput = Input.GetAxis("Horizontal2") * rotationSpeed;
            movementInput = Input.GetAxis("Vertical2");
        }

        if (movementInput != 0)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        // if the key is pressed
        MovePlayer(movementInput);
        RotatePlayer(rotationInput);

        //anim.SetBool("IsMoving", false);
        /*
        Vector3 cameraX = Camera.main.transform.right;
        Vector3 cameraZ = Vector3.Cross(cameraX, Vector3.up);

        inputDirection = camerX * x + cameraZ * y;
        inputDirection.Normalise();
        */
    }

    public void MovePlayer(float movementInput)
    {
        anim.SetFloat("XInput", movementInput);

        Vector3 movement = new Vector3(movementInput, 0f, 0f) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);

        //anim.SetFloat("XInput", animVel)
        // transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    public void RotatePlayer(float rotationInput)
    {
        if (movementInput == 0)
        {
            return;
        }
        // Quaternion target = Quaternion.Euler(0f, rotation, 0f);
        // transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        transform.Rotate(0f, rotationInput, 0f);

        /*
        Quaternion targetDir = Quaternion.LookRotation(inputDirection);
        Quaternion lerpedDir = Quaternion.Lerp(transform.rotation, targetDir, turnSpeed * Time.deltaTime);
        transform.rotation = lerpedDir;
        */
    }
}
