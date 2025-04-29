using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    public enum ItemType { Key, HeartPotion, SpeedPotion, Repellent }
    public ItemType type;
    public int value = 1;

    private Vector3 startPos;

    public string itemName;

    public Item(string name)
    {
        itemName = name;
    }
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
