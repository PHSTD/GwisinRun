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
            m_switchOn = !m_switchOn;
            for (int i = 0; i < m_objectAnimator.Count; i++)
            {
                boolAnimator(m_objectAnimator[i]);
            }
        }        
    }

    // 현 단계에선 번갈아가면서 bool값이 번갈아가게 되어 있으나,
    // bool 변수 세부 값을 조정할 수 있도록 세팅할 건지도 고민중
    private void boolAnimator(Animator animator)
    {
        m_switchOn = !m_switchOn;
        animator.SetBool("IsOpen", m_switchOn);
    }
}
