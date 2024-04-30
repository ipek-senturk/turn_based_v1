using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public string EnemyName { get; set; }
    public int EnemyLevel { get; set; }
    public int EnemyHP { get; set; }
    public int EnemyAtt { get; set; }
    public int EnemyID { get; set; }
    public Vector3 position { get; set; }
    public GameObject EnemyGameObject { get; set; }
}

