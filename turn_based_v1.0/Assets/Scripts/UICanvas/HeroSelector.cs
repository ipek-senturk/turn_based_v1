using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelector : MonoBehaviour
{

    public PartyManager partymanager;
    public UIManager UiManager;
    string selectedName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            selectedName = collision.GetComponent<HeroStats>().warriorData.Name;
            checkId();
        }
    }

    void checkId()
    {
        foreach (var warrior in partymanager.warriorList)
        {
            if (warrior.WarriorName == selectedName)
            {
                UiManager.selectedWarriorId = warrior.WarriorId;
                break;
            }
        }
    }

}