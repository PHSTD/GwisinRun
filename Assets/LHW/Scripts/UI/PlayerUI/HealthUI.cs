using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    Image m_hpImage;

    private int m_maxHealth;
    private int m_currentHealth;

    private PlayerHealth m_playerHealth;

    void Start()
    {
        m_hpImage = GetComponent<Image>();
        m_playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        m_maxHealth = m_playerHealth.GetMaxHealth();
        m_currentHealth = m_playerHealth.GetCurrentHealth();
    }

    void Update()
    {
        m_currentHealth = m_playerHealth.GetCurrentHealth();
        float toFill;
        toFill = (float)m_currentHealth / m_maxHealth;

        m_hpImage.fillAmount = toFill;
    }
}
