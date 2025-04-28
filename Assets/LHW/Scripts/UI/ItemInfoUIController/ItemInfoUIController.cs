using UnityEngine;

public class ItemInfoUIController : MonoBehaviour
{
    [SerializeField] Canvas m_canvas;
    [SerializeField] GameObject m_panel;
    private RectTransform m_panelpos;

    public void Awake()
    {
        m_panelpos = m_panel.GetComponent<RectTransform>();
    }


    private void OnMouseOver()
    {
        m_panel.SetActive(true);

        Vector2 localPos = Vector2.zero;

        RectTransform rectTransform = m_canvas.transform as RectTransform;
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePos, m_canvas.worldCamera, out localPos);
        m_panelpos.anchoredPosition = localPos;
    }

    private void OnMouseExit()
    {
        m_panel.SetActive(false);
    }
}
