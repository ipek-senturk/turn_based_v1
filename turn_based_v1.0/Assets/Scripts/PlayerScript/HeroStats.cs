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
    }

    [SerializeField]
    public WarriorData warriorData;

    public PartyManager partyManager;
    public GameObject HpAppPanel;
    public GameObject mpPanel;
    public GameObject NamePanel;
    public GameObject levelPanel;
    public bool hero;

    private void Start()
    {
        partyManager.addWarriorToList(warriorData, transform.position, gameObject);
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
        // HpAppPanel.gameObject.transform.Find(warriorData.Name + "HP").GetComponent<TextMeshProUGUI>().text = "HP " + warriorData.HP.ToString();
    }
}
