using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public PartyManager partyManager;
    public NpcObject[] enemyObject;
    public GameObject EnemyTemplate;
    public GameObject[] SpawnLocation;
    SpriteRenderer render;
    public int ID;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        render.sprite = enemyObject[0].sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            spawnEnemys();

        partyManager.EnemySpawner = this;
    }
    public void killSpawner()
    {
        Destroy(gameObject);
    }

    void spawnEnemys()
    {
        for (int i = 0; i < enemyObject.Length; i++)
        {
            var newEnemy = Instantiate(EnemyTemplate, SpawnLocation[i].transform.position, Quaternion.identity);
            newEnemy.transform.parent = gameObject.transform;
            newEnemy.GetComponent<EnemyTemplate>().enemydata.HP = enemyObject[i].HP;
            newEnemy.GetComponent<EnemyTemplate>().enemydata.Att = enemyObject[i].Att;
            newEnemy.GetComponent<EnemyTemplate>().enemydata.Name = enemyObject[i].Name;
            newEnemy.GetComponent<EnemyTemplate>().enemydata.id = i;
            
            newEnemy.GetComponent<SpriteRenderer>().sprite= enemyObject[i].sprite;
            newEnemy.gameObject.name = i.ToString();
            newEnemy.GetComponent<EnemyTemplate>().SendData();
        }
        render.sprite = null;
    }
}