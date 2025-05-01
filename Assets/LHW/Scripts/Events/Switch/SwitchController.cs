using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour, IInteractable
{
    [SerializeField] List<Animator> m_objectAnimator;

    public void Interact()
    {
        for (int i = 0; i < m_objectAnimator.Count; i++)
        {
            boolAnimator(m_objectAnimator[i]);
        }
    }

    private void boolAnimator(Animator animator)
    {
        bool isOpen = animator.GetBool("IsOpen");
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
    }
}
