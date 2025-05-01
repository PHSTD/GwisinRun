using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

[Serializable]
public class UsableItem : Item, IUsable
{
    //# 포션 등은 사용했을 때 피가 회복 또는 감소
    [SerializeField] private float value;
    //# 체크 -> +, 아니면 -> -
    [SerializeField] private bool isPositive;
    
    public UsableItem(string name, ItemType type) : base(name, type)
    {
    }
    
    public void Use()
    {
        //# 각 아이템에 따라서 수치 증가 또는 감소
        Debug.Log($"{name} 아이템 사용");
    }
}
