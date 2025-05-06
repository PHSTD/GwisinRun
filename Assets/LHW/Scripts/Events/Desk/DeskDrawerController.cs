using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DeskDrawerController : MonoBehaviour, IInteractable
{
    private Animator m_animator;
    // private Material[] mat = new Material[2];
    // private bool isDetected = false;
    // [SerializeField] private Material m_material1;
    // [SerializeField] private Material m_material2;

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        // mat[0] = m_material1;
        // mat[1] = m_material2;
    }

    private void Update()
    {
        //ChangeMat(isDetected);
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

        /*
        if (isOpen)
        {
            GameManager.Instance.Audio.PlaySound(SoundType.DrawerOpen);
        }
        else
        {
            GameManager.Instance.Audio.PlaySound(SoundType.DrawerClose);
        }
        */
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

    // �ƿ����� ���̴� ���� ������ �켱 �ּ� ó�� (���»��� ������ ����)
    /*
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
    */
}
