﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public PartyManager partyManager;
    public NpcObject[] enemyObject;
    public GameObject[] EnemyTemplate;
    public GameObject[] SpawnLocation;
    SpriteRenderer render;
    public int ID;

    private void Start()
    {
        // Check if the spawner has been defeated before activating
        if (GameManager.Instance.GetDefeatedSpawners().Contains(ID.ToString()))
        {
            gameObject.SetActive(false);
            return;
        }

        render = GetComponent<SpriteRenderer>();
        render.sprite = enemyObject[0].sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            SpawnEnemies();

        partyManager.EnemySpawner = this;
        gameObject.GetComponent<Animator>().enabled = false;
    }

    public void KillSpawner()
    {
        // Add to the list of defeated spawners in GameManager
        GameManager.Instance.AddDefeatedSpawner(ID);
        gameObject.SetActive(false);
        // Destroy(gameObject);
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyObject.Length; i++)
        {
            var newEnemy = Instantiate(EnemyTemplate[i], SpawnLocation[i].transform.position, Quaternion.identity);
            newEnemy.transform.parent = gameObject.transform;
            newEnemy.GetComponent<EnemyTemplate>().enemydata.HP = enemyObject[i].HP;
            newEnemy.GetComponent<EnemyTemplate>().enemydata.Att = enemyObject[i].Att;
            newEnemy.GetComponent<EnemyTemplate>().enemydata.Name = enemyObject[i].Name;
            newEnemy.GetComponent<EnemyTemplate>().enemydata.id = i;
            
            newEnemy.GetComponent<SpriteRenderer>().sprite = enemyObject[i].sprite;
            newEnemy.name = i.ToString();
            newEnemy.GetComponent<EnemyTemplate>().SendData();
        }
        render.sprite = null;
    }
}
