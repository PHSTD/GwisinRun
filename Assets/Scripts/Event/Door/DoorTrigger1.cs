using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger1 : MonoBehaviour
{
    public bool m_playerDetected1 = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_playerDetected1 = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_playerDetected1 = false;
        }
    }

    public bool PlayerDetected()
    {
        return m_playerDetected1;
    }
}
