using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    public bool hero;
    
    [SerializeField] private UIInventory uiInventory;
    
    private void Start()
    {
        warriorData.inventory = new Inventory();
        uiInventory.SetInventory(warriorData.inventory);
        partyManager.AddWarriorToList(warriorData, transform.position, gameObject);
    }

    public void RecieveDamage(int damage)
    {

        warriorData.HP -= damage;
        if (warriorData.HP <= 0)
        {
            if (!hero)
                gameObject.SetActive(false);
            HpAppPanel.gameObject.transform.Find(warriorData.Name+"HP").gameObject.SetActive(false);
            NamePanel.gameObject.transform.Find(warriorData.Name).gameObject.SetActive(false);
        }

        if (warriorData.HP >= 0)
            UpdateUI();

    }
    void UpdateUI()
    {
        HpAppPanel.transform.Find(warriorData.Name + "HP").GetComponent<TextMeshProUGUI>().text = "HP " + warriorData.HP.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if(itemWorld != null)
        {
            // Touching the item
            warriorData.inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }
}
