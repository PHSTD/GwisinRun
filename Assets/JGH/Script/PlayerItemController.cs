using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour, IInteractable
{
    void Update()
    {
        if (GameManager.Instance.Input.InteractionKeyPressed)
        {
            Interact();
        }
    }

    public void Interact()
    {
        Debug.Log("sdf");
    }
}
