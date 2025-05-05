using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    Image m_stImage;

    private int m_maxStamina;
    private int m_currentStamina;

    private PlayerHealth m_playerHealth;

    Color HPColor;

    void Awake()
    {
        m_stImage = GetComponent<Image>();
        m_maxStamina = m_playerHealth.GetMaxStamina();
        m_currentStamina = m_playerHealth.GetCurrentStamina();
    }

    void Update()
    {
        float toFill;
        toFill = (float)m_currentStamina / m_maxStamina;

        m_stImage.fillAmount = toFill;
    }
}
