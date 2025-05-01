using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ItemInfoUIController : MonoBehaviour
{
    [SerializeField] Canvas m_canvasRectTransfom;
    [SerializeField] GameObject m_panel;
    [SerializeField] private TMP_Text m_popupText;
    private RectTransform m_panelpos;
    [SerializeField] private float m_detectRadius;
    [SerializeField] private LayerMask m_playerLayer;

    public void Awake()
    {
        m_panelpos = m_panel.GetComponent<RectTransform>();
    }

    private void UpdateTextMessage()
    {
        var interactionKey = PlayerPrefs.GetString("Interaction");
        if (string.IsNullOrEmpty(interactionKey))
            return;
        
        if (gameObject.CompareTag("Item"))
        {
            m_popupText.text= $"아이템을 얻으려면 [{interactionKey}]를 누르세요.";
        }
        
        else if (gameObject.CompareTag("InteractableObject"))
        {
            m_popupText.text= $"상호작용을 하려면 [{interactionKey}]를 누르세요.";
        }
    }


    private void OnMouseOver()
    {
        if (Physics.OverlapSphere(transform.position, m_detectRadius, m_playerLayer).Length > 0)
        {
            m_panel.SetActive(true);
            UpdateTextMessage();

            Vector3 localPos = Vector3.zero;

            RectTransform rectTransform = m_canvasRectTransfom.transform as RectTransform;
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            

            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, mousePos, m_canvasRectTransfom.worldCamera, out localPos);
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

    private void OnDisable()
    {
        m_panel.SetActive(false);
    }
}
