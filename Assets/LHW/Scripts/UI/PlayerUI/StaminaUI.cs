using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    Image m_stImage;

    private int m_maxStamina;
    private int m_currentStamina;

    private PlayerHealth m_playerHealth;

    void Start()
    {
        m_stImage = GetComponent<Image>();
        m_playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        m_maxStamina = m_playerHealth.GetMaxStamina();
        m_currentStamina = m_playerHealth.GetCurrentStamina();
    }

    void Update()
    {
        m_currentStamina = m_playerHealth.GetCurrentStamina();
        float toFill;
        toFill = (float)m_currentStamina / m_maxStamina;

        m_stImage.fillAmount = toFill;
    }
}
