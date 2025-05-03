using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class IsLockedDoor : MonoBehaviour
{
    private bool isLocked = true;

    public bool IsLocked()
    {
        if(isLocked == false)
        {
            return isLocked;
        }
        TryOpen();
        return isLocked;
    }

    private void TryOpen()
    {
        // RemoveKey()를 가져와서 이게 True면 열리고, 아니면 안열림
        // if(GameManager.Instance.Inventory.RemoveKey())
        // { islocked = false}
    }
}
