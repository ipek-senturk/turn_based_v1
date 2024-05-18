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
            warriorData.inventory = new Inventory();
            warriorData.inventory.AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 4 });
            warriorData.inventory.AddItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 4 });
            GameObject canvasInventory = GameObject.Find("Canvas Inventory");
            uiInventory = Instantiate(uiInventoryPrefab, canvasInventory.transform);
            uiInventory.SetInventory(warriorData.inventory);
        } else
        {
            warriorData.inventory = new Inventory();
        }
        partyManager.AddWarriorToList(warriorData, transform.position, gameObject);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && isMainPlayer && gameObject.GetComponent<MovePlayer>().playerInput.state == InputManager.ControllerState.Movable)
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

    public void RecieveDamage(int damage)
    {
        warriorData.HP -= damage;
        if (warriorData.HP <= 0)
        {
            if (!isMainPlayer)
                gameObject.SetActive(false);
            HpAppPanel.transform.Find(warriorData.Name + "HP").gameObject.SetActive(false);
            NamePanel.transform.Find(warriorData.Name).gameObject.SetActive(false);
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
    void UpdateUI()
    {
        HpAppPanel.transform.Find(warriorData.Name + "HP").GetComponent<TextMeshProUGUI>().text = "HP " + warriorData.HP.ToString();
    }

    void UpdateManaPanel()
    {
        mpPanel.transform.Find(warriorData.Name + "MP").GetComponent<TextMeshProUGUI>().text = "MP " + warriorData.mp.ToString();
    }
    public bool GetIsMainPlayer()
    {
        return isMainPlayer;
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
}
