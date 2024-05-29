using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;
    public string itemID; // Unique identifier for each item spawner

    private static int idCounter = 0; // Static counter to ensure unique IDs

    private void Awake()
    {
        // Assign a unique ID if not already set
        if (string.IsNullOrEmpty(itemID))
        {
            itemID = idCounter.ToString();
            idCounter++;
        }
    }

    private void Start()
    {
        // Check if the item has been picked up before spawning
        if (GameManager.Instance.GetPickedItems().Contains(itemID))
        {
            Destroy(gameObject);
            return;
        }

        // Capture the returned ItemWorld instance
        ItemWorld itemWorld = ItemWorld.SpawnItemWorld(transform.position, item);
        // Set the unique identifier
        itemWorld.itemID = itemID;
        // Destroy the spawner game object
        Destroy(gameObject);
    }
}
