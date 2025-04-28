using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] Animator m_doorAnimator;
    private bool IsOpen = false;
    private bool IsOpen2 = false;
    private bool Close = true;
    [SerializeField] DoorTrigger1 doortrigger1;
    [SerializeField] DoorTrigger2 doortrigger2;

    [SerializeField] GameObject player;

    public void Interact()
    {
        Vector3 m_playerPos = player.transform.position;
        if (Close == true && doortrigger1.PlayerDetected())
        {
            OpenDoorCounterClockwise();
        }
        else if (Close == true && doortrigger2.PlayerDetected())
        {
            OpenDoorClockwise();
        }
        else if (Close == false)
        {
            CloseDoor();
        }
        else
        {
            return;
        }
    }

    private void OpenDoorClockwise()
    {
        IsOpen2 = true;
        Close = false;
        m_doorAnimator.SetBool("IsOpen2", IsOpen2);
        m_doorAnimator.SetBool("Close", Close);
    }

    private void OpenDoorCounterClockwise()
    {
        IsOpen = true;
        Close = false;
        m_doorAnimator.SetBool("IsOpen", IsOpen);
        m_doorAnimator.SetBool("Close", Close);
    }
    private void CloseDoor()
    {
        IsOpen = false;
        IsOpen2 = false;
        Close = true;
        m_doorAnimator.SetBool("IsOpen", IsOpen);
        m_doorAnimator.SetBool("IsOpen2", IsOpen2);
        m_doorAnimator.SetBool("Close", Close);
    }
}
