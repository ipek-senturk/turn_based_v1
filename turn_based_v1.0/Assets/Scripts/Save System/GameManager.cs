using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<string> defeatedSpawners = new List<string>();
    private List<string> pickedItems = new List<string>();
    public bool isNewGame = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddDefeatedSpawner(int spawnerID)
    {
        defeatedSpawners.Add(spawnerID.ToString());
    }

    public void AddPickedItem(string itemID)
    {
        pickedItems.Add(itemID);
    }

    public List<string> GetDefeatedSpawners()
    {
        return defeatedSpawners;
    }

    public List<string> GetPickedItems()
    {
        return pickedItems;
    }
}
