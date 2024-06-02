using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cat : MonoBehaviour
{
    public AudioClip soundEffect;
    public GameObject chatPanel;
    private AudioSource audioSource;
    private bool isPlaying = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Meow()
    {
        Debug.Log("Meow");
        SaveGameState();
        if (!isPlaying)
        {
            isPlaying = true;
            StartCoroutine(ToggleChat());
            StartCoroutine(PlayMeowEffect());
        }
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

    private IEnumerator ToggleChat()
    {
        if (chatPanel != null)
        {
            chatPanel.SetActive(true);

            // Aktif sahne ID'sine göre bekleme süresini belirle
            float waitTime = 2f; // Varsayýlan bekleme süresi
            int sceneID = SceneManager.GetActiveScene().buildIndex;

            if (sceneID == 2)
            {
                waitTime = 5f; // Sahne ID 2 ise 5 saniye bekle
            }
            else if (sceneID == 1)
            {
                waitTime = 2f; // Sahne ID 1 ise 2 saniye bekle
            }

            yield return new WaitForSeconds(waitTime);
            chatPanel.SetActive(false);
        }
    }

    private IEnumerator PlayMeowEffect()
    {
        audioSource.PlayOneShot(soundEffect);
        yield return new WaitForSeconds(soundEffect.length);
        isPlaying = false;
    }


}
