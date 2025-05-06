using UnityEngine;
using UnityEngine.UI;

public class WaitForInputUI : MonoBehaviour
{
    [SerializeField] private Button m_resetButton;
    [SerializeField] private Button m_backButton;

    private void OnEnable()
    {
        m_resetButton.gameObject.SetActive(false);
        m_backButton.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        m_resetButton.gameObject.SetActive(true);
        m_backButton.gameObject.SetActive(true);
    }
}