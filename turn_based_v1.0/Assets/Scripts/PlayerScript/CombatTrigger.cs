using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    public GameObject combatUI;
    public CombatCanvas combatcanvasScript;
    public InputManager InputManager;
    public PartyManager partyManager;

    //public Controllers controllers 


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" )
        {

            partyManager.getHeroStats();
            InputManager.stopPlayer();

            combatUI.SetActive(true);
            Invoke("canvasScript", 0.2f);
            Invoke("startCombatTrigger", 0.5f);

           
        }

    }

    void startCombatTrigger()
    {
        partyManager.startCombat();
    }

    void canvasScript()
    {
        combatcanvasScript.startScript();
    }
}
