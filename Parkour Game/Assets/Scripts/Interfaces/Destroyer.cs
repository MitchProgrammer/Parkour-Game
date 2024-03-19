using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour, IInteractable
{
    
    public void Interact()
    {
        Destroy(gameObject);
    }
}
