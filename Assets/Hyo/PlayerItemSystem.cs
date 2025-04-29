using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemSystem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            bool added = false;

            switch (item.type)
            {
                case Item.ItemType.Key:
                    added = Inventory.Instance.AddItem(new Item("Key"));
                    break;
                case Item.ItemType.HeartPotion:
                    added = Inventory.Instance.AddItem(new Item("HeartPotion"));
                    break;
                case Item.ItemType.SpeedPotion:
                    added = Inventory.Instance.AddItem(new Item("SpeedPotion"));
                    break;
                case Item.ItemType.Repellent:
                    added = Inventory.Instance.AddItem(new Item("Repellent"));
                    break;
                    // 아이템 종류 추가
            }

            if (added)
            {
                Destroy(other.gameObject);
            }
            else
            {
                Debug.Log("인벤토리가 가득 찼습니다.");
            }
        }
    }

}
public class ItemUser : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.Instance.UseItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Inventory.Instance.UseItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Inventory.Instance.UseItem(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Inventory.Instance.UseItem(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Inventory.Instance.UseItem(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Inventory.Instance.UseItem(5);
        }
    }
}
