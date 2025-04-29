using UnityEngine;

public class ItemInfoUIController : MonoBehaviour
{
    [SerializeField] Canvas m_canvas;
    [SerializeField] GameObject m_panel;
    private RectTransform m_panelpos;
    [SerializeField] private float m_detectRadius;
    [SerializeField] private LayerMask m_playerLayer;

    public void Awake()
    {
        m_panelpos = m_panel.GetComponent<RectTransform>();
    }


    private void OnMouseOver()
    {
        if (Physics.OverlapSphere(transform.position, m_detectRadius, m_playerLayer).Length > 0)
        {
            m_panel.SetActive(true);

            Vector2 localPos = Vector2.zero;

            RectTransform rectTransform = m_canvas.transform as RectTransform;
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePos, m_canvas.worldCamera, out localPos);
            m_panelpos.anchoredPosition = localPos;
        }
        else
        {
            m_panel.SetActive(false);
        }
    }

    private void OnMouseExit()
    {
        m_panel.SetActive(false);
    }
}
