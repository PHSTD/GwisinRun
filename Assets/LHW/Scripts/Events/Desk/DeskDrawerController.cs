using UnityEngine;

public class DeskDrawerController : MonoBehaviour, IInteractable
{
    private Animator m_animator;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
    }


    public void Interact()
    {
        DrawerAnimator(m_animator);
    }

    private void DrawerAnimator(Animator animator)
    {
        bool isOpen = animator.GetBool("IsOpen");
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
    }
}
