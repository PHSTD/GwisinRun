using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Coin, HeartPotion, SpeedPotion }
    public ItemType type;
    public int value;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; 
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * 25 * Time.deltaTime);
        
        float newY = Mathf.Sin(1 * Time.time) * 0.2f;
        transform.position = startPos + new Vector3 (0, newY, 0);
    }
}
