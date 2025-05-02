using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] Animator m_doorAnimator;
    private bool m_isOpen = false;
    private bool m_isOpen2 = false;
    private bool m_close = true;
    [SerializeField] DoorTrigger1 m_doortrigger1;
    [SerializeField] DoorTrigger2 m_doortrigger2;

    public void Update()
    {
        if(m_doortrigger1.MonsterDetected())
        {
            OpenDoorCounterClockwise();
        }
        else if (m_doortrigger2.MonsterDetected())
        {
            OpenDoorClockwise();
        }
    }

    public void Interact()
    {
        if (m_close == true && m_doortrigger1.PlayerDetected())
        {
            OpenDoorCounterClockwise();
        }
        else if (m_close == true && m_doortrigger2.PlayerDetected())
        {
            OpenDoorClockwise();
        }
        else if (m_close == false)
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
        m_isOpen2 = true;
        m_close = false;
        m_doorAnimator.SetBool("IsOpen2", m_isOpen2);
        m_doorAnimator.SetBool("Close", m_close);
    }

    private void OpenDoorCounterClockwise()
    {
        m_isOpen = true;
        m_close = false;
        m_doorAnimator.SetBool("IsOpen", m_isOpen);
        m_doorAnimator.SetBool("Close", m_close);
    }
    private void CloseDoor()
    {
        m_isOpen = false;
        m_isOpen2 = false;
        m_close = true;
        m_doorAnimator.SetBool("IsOpen", m_isOpen);
        m_doorAnimator.SetBool("IsOpen2", m_isOpen2);
        m_doorAnimator.SetBool("Close", m_close);
    }
}
