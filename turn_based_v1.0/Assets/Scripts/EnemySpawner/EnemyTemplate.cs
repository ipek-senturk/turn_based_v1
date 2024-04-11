﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyTemplate : MonoBehaviour
{
    public PartyManager partyManager;

    public GameObject EnemyNamePanel;
    public GameObject EnemyHpPanel;

    [System.Serializable]
    public struct EnemyData
    {
        public int enemydata;
        public int Att;
        public string Name;
        public int id;
        public int HP;
    }

    [SerializeField]
    public EnemyData enemydata;

    private void Awake()
    {
        partyManager = GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>();
    }   

    public void TakeDamage(int damage)
    {
        enemydata.HP -= damage;

        if (enemydata.HP <= 0)
        {
            Destroy(EnemyNamePanel.gameObject);
            Destroy(EnemyHpPanel.gameObject);
            Destroy(gameObject);
        }
        if (enemydata.HP > 0)
            updateUi();

    }

    void updateUi()
    {
        EnemyHpPanel.GetComponent<TextMeshProUGUI>().text = enemydata.HP.ToString();
    }

    public void SendData()
    {
        sendCombatData();
    }
    public void sendCombatData()
    {
        partyManager.addEnemyToCombatList(enemydata, transform.position, gameObject);
        Invoke("searchForPanels", 0.5f);
    }

    void searchForPanels()
    {
        EnemyNamePanel = GameObject.FindGameObjectWithTag("EnemyNamesPanel").transform.Find(enemydata.id.ToString()).gameObject;
        EnemyHpPanel = GameObject.FindGameObjectWithTag("EnemyHpPanel").transform.Find(enemydata.id.ToString() + "HP").gameObject;
    }
}
