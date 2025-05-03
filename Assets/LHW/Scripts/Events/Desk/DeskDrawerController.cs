using UnityEngine;

public class DeskDrawerController : MonoBehaviour, IInteractable
{
    private Animator m_animator;
    private Material[] mat = new Material[2];
    private bool isDetected = false;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        gameObject.GetComponent<MeshRenderer>().material = mat[0];
    }


    public void Interact()
    {
        DrawerAnimator(m_animator);
        ChangeMat(isDetected);
    }

    private void DrawerAnimator(Animator animator)
    {
        bool isOpen = animator.GetBool("IsOpen");
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
    }
    private void ChangeMat(bool isDetected)
    {
        if(isDetected)
        {
            gameObject.GetComponent<MeshRenderer>().material = mat[1];
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = mat[0];
        }
    }

    public bool OutlineOn()
    {
        isDetected = true;
        return isDetected;
    }

    public bool OutlineOff()
    {
        isDetected = false;
        return false;
    }
}
