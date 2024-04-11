using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatCanvas : MonoBehaviour
{
    public PartyManager partyManager;

    [Header("Hero")]
    public GameObject HeronameTemplate;
    private GameObject newHeroName;
    public GameObject HeroHpTemplate;
    private GameObject newHp;    
    public GameObject HeroMpTemplate;
    private GameObject newMp;
    public GameObject HeroLevelTemplate;
    private GameObject newLevel;

    [Header("NPC")]
    public GameObject NPCnameTemplate;
    private GameObject newNPCName;
    public GameObject NPCHpAppTemplate;
    private GameObject newNPCHpApp;

    [Header("SpawnData")]

    public GameObject CombatPainel;

    private void Start()
    {
        CombatPainel.SetActive(false);
    }

    public void startScript()
    {
        HeroNameSpawns();
        HpAppinfoSpawn();
        MpinfoSpawn();
        LevelInfoSpawn();
        NpcinfoSpawn();
    }

    void HeroNameSpawns()
    {
        HeronameTemplate.SetActive(true);

        foreach (var warrior in partyManager.warriorList)
        {
            newHeroName = Instantiate(HeronameTemplate, transform);
            newHeroName.transform.SetParent(transform.GetChild(0).GetChild(0), false);
            newHeroName.name = warrior.WarriorName;
            newHeroName.transform.GetComponent<TextMeshProUGUI>().text = warrior.WarriorName;
        }

        HeronameTemplate.SetActive(false);
    }

    void HpAppinfoSpawn()
    {
        HeroHpTemplate.SetActive(true);

        foreach (var warrior in partyManager.warriorList)
        {
            newHp = Instantiate(HeroHpTemplate, transform);
            newHp.transform.SetParent(transform.GetChild(0).GetChild(1), false);
            newHp.name = warrior.WarriorName + "HP";
            newHp.transform.GetComponent<TextMeshProUGUI>().text = "HP " + warrior.WarriorHP.ToString();
        }
        HeroHpTemplate.SetActive(false);
    }

    void MpinfoSpawn()
    {
        HeroMpTemplate.SetActive(true);

        foreach (var warrior in partyManager.warriorList)
        {
            newMp = Instantiate(HeroMpTemplate, transform);
            newMp.transform.SetParent(transform.GetChild(0).GetChild(3), false);
            newMp.name = warrior.WarriorName + "MP";
            newMp.transform.GetComponent<TextMeshProUGUI>().text = "MP " + warrior.WarriorMp.ToString();
        }
        HeroMpTemplate.SetActive(false);
    }
    void LevelInfoSpawn()
    {
        HeroLevelTemplate.SetActive(true);

        foreach (var warrior in partyManager.warriorList)
        {
            newLevel = Instantiate(HeroLevelTemplate, transform);
            newLevel.transform.SetParent(transform.GetChild(0).GetChild(2), false);
            newLevel.name = warrior.WarriorName + "Lvl";
            newLevel.transform.GetComponent<TextMeshProUGUI>().text = "Lvl " + warrior.WarriorLevel.ToString();
        }
        HeroLevelTemplate.SetActive(false);
    }
    void NpcinfoSpawn()
    {
        NPCnameTemplate.SetActive(true);
        NPCHpAppTemplate.SetActive(true);

        foreach (var enemy in partyManager.EnemyCombatList)
        {

            newNPCName = Instantiate(NPCnameTemplate, transform);
            newNPCName.transform.SetParent(transform.GetChild(1).GetChild(0), false);
            newNPCName.name = enemy.EnemyID.ToString();
            newNPCName.transform.GetComponent<TextMeshProUGUI>().text = enemy.EnemyName;
        }
        foreach (var enemy in partyManager.EnemyCombatList)
        {
            newNPCHpApp = Instantiate(NPCHpAppTemplate, transform);
            newNPCHpApp.transform.SetParent(transform.GetChild(1).GetChild(1), false);
            newNPCHpApp.name = enemy.EnemyID + "HP";
            newNPCHpApp.transform.GetComponent<TextMeshProUGUI>().text = "HP " + enemy.EnemyHP.ToString();
        }

        NPCHpAppTemplate.SetActive(false);
        NPCnameTemplate.SetActive(false);
    }

    public void destroychildrens()
    {
        foreach (Transform child in gameObject.transform.GetChild(0).GetChild(0))
        {
            if (child.gameObject.activeInHierarchy)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (Transform child in gameObject.transform.GetChild(0).GetChild(1))
        {
            if (child.gameObject.activeInHierarchy)
            {
                Destroy(child.gameObject);
            }
        }
        /////////////////////////////// NEW /////////////////////////////////////////
        foreach (Transform child in gameObject.transform.GetChild(0).GetChild(2))
        {
            if (child.gameObject.activeInHierarchy)
            {
                Destroy(child.gameObject);
            }
        }
        //////////////////////////////////////////////////////////////////////////////
        foreach (Transform child in gameObject.transform.GetChild(1).GetChild(0))
        {
            if (child.gameObject.activeInHierarchy)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (Transform child in gameObject.transform.GetChild(1).GetChild(1))
        {
            if (child.gameObject.activeInHierarchy)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
