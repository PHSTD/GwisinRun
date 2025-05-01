using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHover : MonoBehaviour
{
    void OnMouseEnter()
    {
        Debug.Log("마우스가 아이템 위에 있음");
    }

    void OnMouseExit()
    {
        Debug.Log("마우스가 아이템에서 벗어남");
    }
}
