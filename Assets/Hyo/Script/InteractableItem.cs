using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public Item.ItemType itemType; // 인스펙터에서 선택 가능
    private string itemName;       // 자동으로 설정됨

    public float interactDistance = 2f; // 플레이어와 거리
    private bool isMouseOver = false;

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // 플레이어 위치 찾기
        itemName = itemType.ToString();
    }

    void Update()
    {
        if (isMouseOver && Vector3.Distance(transform.position, player.position) <= interactDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Item newItem = new Item(itemName);
                newItem.type = itemType;

                bool added = Inventory.Instance.AddItem(newItem); // 인벤토리에 추가
                if (added)
                {
                    Destroy(gameObject); // 아이템 삭제
                }
                else
                {
                    Debug.Log("인벤토리가 가득 찼습니다.");
                }
            }
        }
    }

    void OnMouseEnter()
    {
        isMouseOver = true;
    }

    void OnMouseExit()
    {
        isMouseOver = false;
    }
}

