using UnityEngine;

public class DeskDrawerController : MonoBehaviour, IInteractable
{
    [SerializeField] Animator m_drawerAnimator1;
    [SerializeField] Animator m_drawerAnimator2;
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 0.1f);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Debug.Log(hitInfo.collider.gameObject.name);
                DrawerAnimator(hitInfo.collider.gameObject.GetComponent<Animator>());
            }
        }
    }

    private void DrawerAnimator(Animator animator)
    {
        bool isOpen = animator.GetBool("IsOpen");
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
    }
}
