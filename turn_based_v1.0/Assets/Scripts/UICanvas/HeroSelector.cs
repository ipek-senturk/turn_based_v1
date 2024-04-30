using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HeroSelector : MonoBehaviour
{

    public PartyManager partymanager;
    public UIManager UIManager;
    string selectedName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            selectedName = collision.GetComponent<HeroStats>().warriorData.Name;
            CheckID();
            Debug.Log("Selected warrior: " + selectedName + " ID: " + collision.GetComponent<HeroStats>().warriorData.Name);
        }
    }

    void CheckID()
    {
        foreach (var warrior in partymanager.warriorList)
        {
            if (warrior.WarriorName == selectedName)
            {
                UIManager.selectedWarriorId = warrior.WarriorId;
                break;
            }
        }
    }

}