using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.tvOS;

[Serializable]
public class UsableItem : Item, IUsable
{
    //# 포션 등은 사용했을 때 피가 회복 또는 감소
    [SerializeField] private int m_value;
    //# 체크 -> +, 아니면 -> -
    [SerializeField] private bool m_isPositive;
    public int Value => m_isPositive == true ? m_value : -m_value;
    
    public UsableItem(string name, ItemType type) : base(name, type)
    {
    }
    
    public void Use()
    {
        //# 각 아이템에 따라서 수치 증가 또는 감소
        Debug.Log($"{name} 아이템 사용");
        Destroy(gameObject);
    }
}
