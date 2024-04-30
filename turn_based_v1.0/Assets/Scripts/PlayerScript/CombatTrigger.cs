using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    public GameObject combatUI;
    public CombatCanvas combatcanvasScript;
    public InputManager InputManager;
    public PartyManager partyManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            partyManager.GetHeroStats();
            InputManager.StopPlayer();

            combatUI.SetActive(true);
            Invoke(nameof(CanvasScript), 0.2f);
            Invoke(nameof(StartCombatTrigger), 0.5f);
        }
    }

    void StartCombatTrigger()
    {
        partyManager.StartCombat();
    }

    void CanvasScript()
    {
        combatcanvasScript.StartScript();
    }
}
