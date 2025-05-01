using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInfoUI_ : MonoBehaviour
{
    [SerializeField] private GameObject tooltipPanel; // UI Panel 오브젝트
    [SerializeField] private TextMeshProUGUI tooltipText; // Panel 안의 Text
    [SerializeField] private Canvas canvas; // UI가 있는 Canvas

    private bool isHovering = false;
    private float hoverTime = 0f;
    [SerializeField] private float tooltipDelay = 1f;

    void Update()
    {
        if (isHovering)
        {
            hoverTime += Time.deltaTime;

            if (hoverTime >= tooltipDelay)
            {
                if (!tooltipPanel.activeSelf)
                    tooltipPanel.SetActive(true);

                tooltipText.text = "아이템 이름"; // 나중에 변수로 바꿀 수 있음

                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    Input.mousePosition,
                    canvas.worldCamera,
                    out pos
                );
                tooltipPanel.GetComponent<RectTransform>().anchoredPosition = pos;
            }
        }
        else
        {
            if (tooltipPanel.activeSelf)
                tooltipPanel.SetActive(false);

            hoverTime = 0f;
        }
    }

    private void OnMouseEnter()
    {
        isHovering = true;
    }

    private void OnMouseExit()
    {
        isHovering = false;
    }
}

