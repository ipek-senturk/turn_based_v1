using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossairSelector : MonoBehaviour
{
    public UIManager UiManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyTemplate>())
            UiManager.targetID = (collision.gameObject.GetComponent<EnemyTemplate>().enemydata.id);
        print("Selected enemy: " + collision.gameObject.GetComponent<EnemyTemplate>().enemydata.id);

    }

}
