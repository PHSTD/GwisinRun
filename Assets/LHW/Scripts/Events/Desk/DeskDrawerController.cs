using UnityEngine;

public class DeskDrawerController : MonoBehaviour, IInteractable
{
    private bool m_interactable = false;



    public void Interact()
    {
        DrawerAnimator(GetComponent<Animator>());
    }

    private void DrawerAnimator(Animator animator)
    {
        bool isOpen = animator.GetBool("IsOpen");
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
    }
}
