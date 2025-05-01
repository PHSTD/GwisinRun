using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour, IInteractable
{
    [SerializeField] List<Animator> m_objectAnimator;
    private bool m_switchOn = false;
    private bool m_interactable = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            m_interactable = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            m_interactable = false;
    }

    public void Interact()
    {
        if (m_interactable)
        {
            for (int i = 0; i < m_objectAnimator.Count; i++)
            {
                boolAnimator(m_objectAnimator[i]);
            }
        }        
    }

    private void boolAnimator(Animator animator)
    {
        m_switchOn = !m_switchOn;
        animator.SetBool("IsOpen", m_switchOn);
    }
}
