using UnityEngine;

public class OutlineController : MonoBehaviour
{
    private Material[] mat = new Material[2];
    private bool isDetected = false;
    [SerializeField] private Material m_material1;
    [SerializeField] private Material m_material2;
    [SerializeField] private MeshRenderer m_renderer;

    void Start()
    {
        mat[0] = m_material1;
        mat[1] = m_material2;
    }

    private void Update()
    {
        ChangeMat(isDetected);
    }
    private void ChangeMat(bool isDetected)
    {
        if (isDetected)
        {
            m_renderer.material = mat[1];
        }
        else
        {
            m_renderer.material = mat[0];
        }
    }

    public bool OutlineOn()
    {
        isDetected = true;
        return isDetected;
    }

    private void OnMouseExit()
    {
        isDetected = false;
    }
}
