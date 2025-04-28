using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int coin;
    public int heartPotion;
    public int speedPotion;

    public int maxCoin;
    public int maxHeartPotion;
    public int maxSpeedPotion;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.ItemType.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
                case Item.ItemType.HeartPotion:
                    heartPotion += item.value;
                    if (heartPotion > maxHeartPotion)
                        heartPotion = maxHeartPotion;
                    break;
                case Item.ItemType.SpeedPotion:
                    speedPotion += item.value;
                    if (speedPotion > maxSpeedPotion)
                        speedPotion = maxSpeedPotion;
                    break;
            }
            Destroy(other.gameObject);
        }
    }
}
