using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cat : MonoBehaviour
{
    public void Meow()
    {
        Debug.Log("Meow");
        SaveGameState();
    }

    private void SaveGameState()
    {
        GameData gameData = new GameData();
        gameData.currentScene = SceneManager.GetActiveScene().name;
        gameData.heroesData = new List<HeroData>();

        foreach (HeroStats hero in FindObjectsOfType<HeroStats>())
        {
            HeroData heroData = new HeroData();
            heroData.name = hero.warriorData.Name;
            heroData.health = hero.warriorData.HP;
            heroData.mana = hero.warriorData.mp;
            heroData.positionX = hero.transform.position.x;
            heroData.positionY = hero.transform.position.y;
            gameData.heroesData.Add(heroData);
        }

        HeroStats mage = null;
        foreach (HeroStats hero in FindObjectsOfType<HeroStats>())
        {
            if (hero.GetIsMainPlayer())
            {
                mage = hero;
                break;
            }
        }

        if (mage != null)
        {
            gameData.mageInventory = new InventoryData();
            gameData.mageInventory.items = mage.warriorData.inventory.GetItemList();
        }

        gameData.defeatedSpawners = GameManager.Instance.GetDefeatedSpawners();
        gameData.pickedItems = GameManager.Instance.GetPickedItems();

        SaveSystem.SaveGame(gameData);
    }
}
