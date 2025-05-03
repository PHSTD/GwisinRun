using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{

    private PlayerController m_playerController;
    private void Start()
    {
        GameManager.Instance.Inventory.OnUseItem.AddListener(UseItem);
    }

    void Update()
    {
        //# 수정 사항(20250502) -- 시작
        if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
            return;
        //# 수정 사항(20250502) -- 끝
        
        //# 수정 사항(20250503) -- 시작
        if(GameManager.Instance.Input.UseItemKeyPressed)
        {
            GameManager.Instance.Inventory.UseItem();
        }
        //# 수정 사항(20250503) -- 끝
        
        if (GameManager.Instance.Input.DropKeyPressed)
        {
            DropItem();
        }
    }
    
    public void UseItem(string itemName, int value)
    {
        //# 수정 사항(20250502) -- 시작
        if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
            return;
        //# 수정 사항(20250502) -- 끝
        
        switch (itemName)
        {
            case "SpeedPotion":
                PlayerHealth.CurrentStamina += value;

                if (PlayerHealth.CurrentStamina < 0)
                {
                    PlayerHealth.CurrentStamina = 0;
                }
                else if (PlayerHealth.CurrentStamina > PlayerHealth.MaxStamina)
                {
                    PlayerHealth.CurrentStamina = PlayerHealth.MaxStamina;
                }
                break;
            
            case "HeartPotion":
                PlayerHealth.CurrentHealth += value;

                if (PlayerHealth.CurrentHealth < 0)
                {
                    PlayerHealth.CurrentHealth = 0;
                    PlayerController.Die();
                }
                else if (PlayerHealth.CurrentHealth > PlayerHealth.MaxHealth)
                {
                    PlayerHealth.CurrentHealth = PlayerHealth.MaxHealth;
                }
                break;
                
            default: 
                
                break;
        }
    }
    
    void DropItem()
    {
        if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
            return;

        // 현재 선택된 아이템 인덱스 가져오기
        var inventory = GameManager.Instance.Inventory;
        int index = inventory.GetSelectedIndex(); 

        if (index < 0)
        {
            Debug.Log("선택된 아이템 없음");
            return;
        }

        var item = inventory.GetItemAt(index); 
        if (item == null)
        {
            Debug.Log("아이템이 비어 있음");
            return;
        }

        Transform playerTransform = PlayerController.PlayerTransform.transform;
        Vector3 dropPos = playerTransform.position + playerTransform.forward * 1.5f + Vector3.up * 0.5f;

        item.transform.position = dropPos;
        item.transform.rotation = Quaternion.identity;
        item.gameObject.SetActive(true);

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(playerTransform.forward * 2f + Vector3.up * 2f, ForceMode.Impulse);
        }

        inventory.ClearItemAt(index);
        Debug.Log($"{item.name}을 버림");

    }
}
