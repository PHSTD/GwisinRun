using System.Collections;
using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{

    private PlayerController m_playerController;
    private PlayerHealth m_playerHealth;
    
    private void Start()
    {
        GameManager.Instance.Inventory.OnUseItem.AddListener(UseItem);
        
        m_playerHealth = GetComponent<PlayerHealth>();
        m_playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
            return;
        
        if(GameManager.Instance.Input.UseItemKeyPressed)
        {
            GameManager.Instance.Inventory.UseItem();
        }
    }
    
    public void UseItem(string itemName, int value)
    {
        if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
            return;
        
        switch (itemName)
        {
            case "StaminaPotion":
                int getCurrStamina = m_playerHealth.GetCurrentStamina();
                getCurrStamina += value;
                m_playerHealth.SetCurrentStamina(getCurrStamina);

                if (getCurrStamina < 0)
                {
                    m_playerHealth.SetCurrentStamina(0);
                }
                else if (getCurrStamina > m_playerHealth.GetMaxStamina())
                {
                    m_playerHealth.SetCurrentStamina(m_playerHealth.GetMaxStamina());
                }
                break;
            
            case "HeartPotion":
                int currHealth = m_playerHealth.GetCurrentHealth();
                currHealth += value;
                m_playerHealth.SetCurrentHealth(currHealth);

                if (currHealth < 0)
                {
                    m_playerHealth.SetCurrentHealth(0);
                    m_playerController.Die();
                }
                else if (currHealth > m_playerHealth.GetMaxHealth())
                {
                    m_playerHealth.SetCurrentHealth(m_playerHealth.GetMaxHealth());
                }
                break;
            
            case "SpeedPotion":
                StartCoroutine(ItemSpeed(value));
                break;
            default: 
                
                break;
        }
    }

    IEnumerator ItemSpeed(float value)
    {
        PlayerMove.ItemSpeed = value;
        WaitForSeconds time = new WaitForSeconds(0.1f);

        if (value > 0)
        {
            while (PlayerMove.ItemSpeed > 0f)
            {
                yield return time;
                PlayerMove.ItemSpeed -= 0.1f;
            }
        }
        else if (value < 0)
        {
            while (PlayerMove.ItemSpeed < 0f)
            {
                yield return time;
                PlayerMove.ItemSpeed += 0.1f;
            }
        }
    }
}
