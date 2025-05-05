using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    [SerializeField] private Renderer[] m_renderers;
    
    private bool m_isDetected = false;
    
    public bool IsDetected => m_isDetected;

    void Start()
    {
        if (m_renderers == null || m_renderers.Length == 0)
            m_renderers = GetComponentsInChildren<Renderer>();
    } 
    
    private void Update()
    {
        if (GameManager.Instance.Input.InteractionKeyPressed)
        {
            if (m_isDetected)
                Unhide();
            else
                Hide();
        }
        
        if (CanStandUp())
        {
            Clear();
        }
        else
        {
            DetectedObjectAtHead();
        }
    }
    
    void Hide()
    {
        m_isDetected = true;
        SetRenderers(false);
        Debug.Log("플레이어가 숨었습니다.");
    }

    void Unhide()
    {
        m_isDetected = false;
        SetRenderers(true);
        Debug.Log("플레이어가 다시 나타났습니다.");
    }

    void SetRenderers(bool visible)
    {
        foreach (var r in m_renderers)
            r.enabled = visible;
    }
    
    private bool CanStandUp()
    {
        // 플레이어 머리 위 공간 체크
        float checkRadius = 0.6f;
        Vector3 checkPosition = transform.position + Vector3.up;
        return !Physics.CheckSphere(checkPosition, checkRadius);
    }

    void OnEnable()
    {
        m_isDetected = false;
    }
    
    void DetectedObjectAtHead()
    {
        m_isDetected = true;
    }

    void Clear()
    {
        m_isDetected = false;
    }
    
}
