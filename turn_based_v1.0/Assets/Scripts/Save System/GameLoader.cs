using UnityEngine;
using System.Linq;

public class GameLoader : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        if (!GameManager.Instance.isNewGame || GameManager.Instance.isTransitioning)
        {
            Debug.Log("Loading Game State");
            LoadGameState();
            GameManager.Instance.isTransitioning = false; // Reset transition flag after loading
        }
        else
        {
            Debug.Log("New Game");
        }
    }

    private void LoadGameState()
    {
        GameData data = SaveSystem.LoadGame();

        foreach (HeroStats hero in FindObjectsOfType<HeroStats>())
        {
            HeroData heroData = data.heroesData.Find(h => h.name == hero.warriorData.Name);
            if (heroData != null)
            {
                Debug.Log($"Loading hero data for {heroData.name}");
                hero.SetHeroData(heroData);
            }
        }

        HeroStats mage = FindObjectsOfType<HeroStats>().FirstOrDefault(h => h.GetIsMainPlayer());
        if (mage != null)
        {
            if (mage.warriorData.inventory == null)
            {
                mage.warriorData.inventory = new Inventory(mage.UseItem);
            }
            mage.warriorData.inventory.Clear();
            foreach (Item item in data.mageInventory.items)
            {
                mage.warriorData.inventory.AddItem(item);
            }
        }

        foreach (string spawnerID in data.defeatedSpawners)
        {
            Spawner spawner = FindObjectsOfType<Spawner>().FirstOrDefault(s => s.ID.ToString() == spawnerID);
            if (spawner != null)
            {
                spawner.KillSpawner();
            }
        }

        foreach (string itemID in data.pickedItems)
        {
            Debug.Log($"Removing item with ID: {itemID}");
            ItemWorld itemWorld = FindObjectsOfType<ItemWorld>().FirstOrDefault(iw => iw.itemID == itemID);
            if (itemWorld != null)
            {
                itemWorld.DestroySelf();
            }
        }
    }
}
