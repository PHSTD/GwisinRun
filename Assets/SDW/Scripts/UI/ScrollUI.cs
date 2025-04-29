using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollUI : MonoBehaviour
{
    private ScrollRect m_scrollRect;

    void Awake()
    {
        m_scrollRect = GetComponent<ScrollRect>();
    }
    
    
    void Start()
    {
        StartCoroutine(SetScrollPositionTop());
    }

    IEnumerator SetScrollPositionTop()
    {
        yield return null;
        m_scrollRect.verticalNormalizedPosition = 1f;
    }
}
