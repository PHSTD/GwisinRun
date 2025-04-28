using UnityEngine;

public class SwitchController : MonoBehaviour, IInteractable
{
    [SerializeField] Animator m_cageAnimator;
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
        m_switchOn = !m_switchOn;
        m_cageAnimator.SetBool("IsOpen", m_switchOn);
    }
}
