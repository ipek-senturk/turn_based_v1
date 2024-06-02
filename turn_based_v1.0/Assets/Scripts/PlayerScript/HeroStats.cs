using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static HeroStats;
public class HeroStats : MonoBehaviour
{
    [System.Serializable]
    public struct WarriorData
    {
        public int HP;
        public int mp;
        public int level;
        public string Name;
        public int ATT;
        public List<Magic> magicList;
        public Inventory inventory;
    }

    [SerializeField]
    public WarriorData warriorData;
    public PartyManager partyManager;
    public GameObject HpAppPanel;
    public GameObject mpPanel;
    public GameObject NamePanel;
    public GameObject levelPanel;
    public GameObject spellPanel;
    [SerializeField] bool isMainPlayer;
    
    [SerializeField] private UIInventory uiInventoryPrefab;
    [SerializeField] private UIInventory uiInventory;

    private void Start()
    {
        if (gameObject.CompareTag("Mage"))
        {
            isMainPlayer = true;
        }
        if (isMainPlayer)
        {
            warriorData.inventory = new Inventory(UseItem);
            warriorData.inventory.AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 4 });
            warriorData.inventory.AddItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 4 });
            GameObject canvasInventory = GameObject.Find("Canvas Inventory");
            uiInventory = Instantiate(uiInventoryPrefab, canvasInventory.transform);
            uiInventory.SetInventory(warriorData.inventory);
        } else
        {
            warriorData.inventory = new Inventory(UseItem);
        }
        partyManager.AddWarriorToList(warriorData, transform.position, gameObject);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && isMainPlayer)
        {
            Debug.Log("pressed i");

            GameObject background = uiInventory.transform.gameObject;
            if (background.activeSelf)
            {
                uiInventory.Hide();
            }
            else
            {
                uiInventory.Show();
            }
        }
    }

    public void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                warriorData.inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                break;
            case Item.ItemType.ManaPotion:
                warriorData.inventory.RemoveItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
                break;
        }
    }

    public void ReceiveDamage(int damage)
    {
        warriorData.HP -= damage;
        if (warriorData.HP <= 0)
        {
            if (isMainPlayer)
            {
                warriorData.HP = 1;
            }
            else
            {
                gameObject.SetActive(false);
                HpAppPanel.transform.Find(warriorData.Name + "HP").gameObject.SetActive(false);
                mpPanel.transform.Find(warriorData.Name + "MP").gameObject.SetActive(false);
                NamePanel.transform.Find(warriorData.Name).gameObject.SetActive(false);
                levelPanel.transform.Find(warriorData.Name + "Lvl").gameObject.SetActive(false);
            }
        }

        if (warriorData.HP >= 0)
            UpdateUI();
    }

    public void CastSpell(int manaCost)
    {
        warriorData.mp -= manaCost;
        if(warriorData.mp < 0)
        {
            warriorData.mp = 0;
        }
        UpdateManaPanel();
    }
    public void SetHeroData(HeroData data)
    {
        warriorData.HP = data.health;
        warriorData.mp = data.mana;
        transform.position = new Vector3(data.positionX, data.positionY, transform.position.z);
    }
    public void UpdateUI()
    {
        int maxHP = 100;
        HpAppPanel.transform.Find(warriorData.Name + "HP").GetComponent<TextMeshProUGUI>().text = "HP " + warriorData.HP.ToString();
        if(warriorData.HP > maxHP)
        {
            HpAppPanel.transform.Find(warriorData.Name + "HP").GetComponent<TextMeshProUGUI>().text = "HP " + maxHP;
        }
        mpPanel.transform.Find(warriorData.Name + "MP").GetComponent<TextMeshProUGUI>().text = "MP " + warriorData.mp.ToString();
    }

    void UpdateManaPanel()
    {
        mpPanel.transform.Find(warriorData.Name + "MP").GetComponent<TextMeshProUGUI>().text = "MP " + warriorData.mp.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<ItemWorld>(out var itemWorld) && GetIsMainPlayer())
        {
            // Touching the item
            warriorData.inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }
    public bool GetIsMainPlayer()
    {
        return isMainPlayer;
    }

    public void SetIsMainPlayer(bool isMainPlayer)
    {
        this.isMainPlayer = isMainPlayer;
    }
}
