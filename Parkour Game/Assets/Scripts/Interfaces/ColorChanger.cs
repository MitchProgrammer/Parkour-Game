using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GetComponent<Renderer>().material.color = Color.cyan;
    }
}
