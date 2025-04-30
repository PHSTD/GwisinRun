using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteraction : MonoBehaviour
{
    public string itemName = "Key"; // 아이템 이름
    public float showInfoDelay = 1.5f; // 이름 보이기까지 걸리는 시간
    public float detectRadius = 3f; // 플레이어 감지 거리

    private bool isMouseOver = false;
    private bool isPlayerNearby = false;
    private Coroutine showInfoCoroutine;

    public GameObject namePanel; // UI 패널 (Text 포함)
    public Text nameText; // 아이템 이름 표시할 텍스트
    public Transform player; // 플레이어 트랜스폼

    void Start()
    {
        namePanel.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        isPlayerNearby = distance <= detectRadius;
    }

    void OnMouseEnter()
    {
        isMouseOver = true;

        if (isPlayerNearby)
        {
            showInfoCoroutine = StartCoroutine(ShowItemInfoAfterDelay());
        }
    }

    void OnMouseExit()
    {
        isMouseOver = false;

        if (showInfoCoroutine != null)
        {
            StopCoroutine(showInfoCoroutine);
        }

        namePanel.SetActive(false);
    }

    IEnumerator ShowItemInfoAfterDelay()
    {
        yield return new WaitForSeconds(showInfoDelay);

        if (isMouseOver && isPlayerNearby)
        {
            namePanel.SetActive(true);
            nameText.text = itemName;
        }
    }
}

