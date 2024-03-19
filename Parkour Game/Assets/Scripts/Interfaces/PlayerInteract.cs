using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public IInteractable targetScript;

    //get a reference to an interactable you are close to
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null)
        {
            targetScript = other.GetComponent<IInteractable>();
        }
    }
    //remove the reference to the interactable if you are exiting
    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<IInteractable>() == targetScript)
        {
            targetScript = null;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && targetScript != null)
        {
            //play animation
            //play sound

            //run script
            targetScript.Interact();
        }
    }
}
