using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectMovement : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float rotationSpeed;

    void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        float newY = Mathf.Sin(verticalSpeed * Time.time) * 0.2f;
        transform.position = startPos + new Vector3(0, newY, 0);
    }
}
