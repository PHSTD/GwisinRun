using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{

    void Update()
    {
        //# 수정 사항(20250502) -- 시작
        // if (GameManager.Instance.IsPaused || GameManager.Instance.IsCleared || GameManager.Instance.IsGameOver)
        //     return;
        //# 수정 사항(20250502) -- 끝
        
        for (int i = 0; i < GameManager.Instance.Input.ItemKeyPressed.Length; i++)
        {
            if (GameManager.Instance.Input.ItemKeyPressed[i])
            {
                GameManager.Instance.Inventory.UseItem(i);
                //# 현재 방법을 찾지 못해 읽은 후 수동으로 clear 해야 합니다.
                //# 추후 리팩토링 예정
                GameManager.Instance.Input.ItemKeyPressed[i] = false;
            }
        }
    }
    
}
