using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
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
        DrawerSound();
        StartCoroutine(DrawerAnimator(m_animator));
    }

    IEnumerator DrawerAnimator(Animator animator)
    {        
        bool isOpen = animator.GetBool("IsOpen");
        isOpen = !isOpen;
        if(isOpen)
        {
            yield return new WaitForSeconds(0.2f);
            animator.SetBool("IsOpen", isOpen);
        }
        else
        {
            yield return new WaitForSeconds(0.7f);
            animator.SetBool("IsOpen", isOpen);
        }
    }

    private void DrawerSound()
    {
        bool isOpen = m_animator.GetBool("IsOpen");
        if (!isOpen)
        {
            GameManager.Instance.Audio.PlaySound(SoundType.DrawerOpen);
        }
        else
        {
            GameManager.Instance.Audio.PlaySound(SoundType.DrawerClose);
        }
    }
}
