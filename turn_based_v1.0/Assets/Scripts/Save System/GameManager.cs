using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<string> defeatedSpawners = new List<string>();
    private List<string> pickedItems = new List<string>();
    public bool isNewGame = true;
    public bool isTransitioning = false;
    public GameData currentGameData;

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

    public void SaveGameState()
    {
        currentGameData = new GameData
        {
            currentScene = SceneManager.GetActiveScene().name,
            heroesData = new List<HeroData>(),
            mageInventory = new InventoryData { items = new List<Item>() },
            defeatedSpawners = new List<string>(defeatedSpawners),
            pickedItems = new List<string>(pickedItems)
        };

        foreach (HeroStats hero in FindObjectsOfType<HeroStats>())
        {
            HeroData heroData = new HeroData
            {
                name = hero.warriorData.Name,
                health = hero.warriorData.HP,
                mana = hero.warriorData.mp,
                positionX = hero.transform.position.x,
                positionY = hero.transform.position.y
            };
            currentGameData.heroesData.Add(heroData);

            if (hero.GetIsMainPlayer())
            {
                currentGameData.mageInventory.items = hero.warriorData.inventory.GetItemList();
            }
        }

        SaveSystem.SaveGame(currentGameData);
    }
}
