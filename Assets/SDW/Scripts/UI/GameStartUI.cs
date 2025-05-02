using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
    }


    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.IsPaused = false;
    }
}
