using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Player Movement
    public float movementSpeed;
    public float rotationSpeed;

    public bool canDash;
    public float dashCooldown;

    public float rotationInput;
    public float movementInput;

    // Rigidbogy
    public Rigidbody rb;

    // Player Identification
    public enum Players { player1, player2 }
    public Players player;

    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("PlayerMovement.cs loaded successfully");

        canDash = true;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public void FixedUpdate()
    {
        if (player == Players.player1)
        {
            rotationInput = Input.GetAxis("Horizontal") * rotationSpeed;
            movementInput = Input.GetAxis("Vertical");
        }
        else
        {
            rotationInput = Input.GetAxis("Horizontal2") * rotationSpeed;
            movementInput = Input.GetAxis("Vertical2");
        }


        // if the key is pressed
        MovePlayer(movementInput);
        RotatePlayer(rotationInput);

        /*
        Vector3 cameraX = Camera.main.transform.right;
        Vector3 cameraZ = Vector3.Cross(cameraX, Vector3.up);

        inputDirection = camerX * x + cameraZ * y;
        inputDirection.Normalise();
        */
    }

    public IEnumerator Dash()
    {
        movementSpeed += 5;

        canDash = false;

        yield return new WaitForSeconds(1);

        movementSpeed -= 5;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    public void MovePlayer(float movementInput)
    {
        Vector3 movement = new Vector3(movementInput, 0f, 0f) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);

        // transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }

    public void RotatePlayer(float rotationInput)
    {
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
