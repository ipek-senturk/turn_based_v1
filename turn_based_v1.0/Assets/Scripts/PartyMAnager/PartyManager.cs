using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public List<Warrior> warriorList = new List<Warrior>();
    public List<Enemy> EnemyCombatList = new List<Enemy>();

    public UIManager UIManager;
    public InputManager inputManager;

    public CombatCanvas combatcanvasScript;
    public Input playerInput;
    public spawner EnemySpawner;
    public GameObject CombatPanel;
    public HeroStats heroStats;

    private int heroturncount = 0;
    private bool playersTurn = true;

    public void AddWarriorToList(HeroStats.WarriorData warriorData, Vector3 position, GameObject warriorGameObject)
    {
        warriorList.Add(new Warrior
        {
            WarriorName = warriorData.Name,
            WarriorHP = warriorData.HP,
            WarriorMp = warriorData.mp,
            WarriorLevel = warriorData.level,
            WarriorPosition = position,
            WarriorGameObject = warriorGameObject,
            WarriorAttack = warriorData.ATT,
            WarriorId = warriorList.Count,
            MagicList = warriorData.magicList
        });
    }

    public void AddEnemyToCombatList(EnemyTemplate.EnemyData enemyData, Vector3 pos, GameObject gameObject)
    {
        EnemyCombatList.Add(new Enemy
        {
            EnemyName = enemyData.Name,
            EnemyHP = enemyData.HP,
            EnemyID = EnemyCombatList.Count,
            position = pos,
            EnemyGameObject = gameObject,
            EnemyAtt = enemyData.Att
        });
    }
    public void StartCombat()
    {        
        // Start combat with players' turn
        playersTurn = true;
        UIManager.gameObject.SetActive(true);
        UIManager.StartUIManager();
    }
    public void GetHeroStats()
    {
        foreach (var warrior in warriorList)
        {
            warrior.WarriorPosition = warrior.WarriorGameObject.GetComponent<Transform>().position;
        }
    }

    public bool GetIsPlayersTurn()
    {
        return playersTurn;
    }

    public List<Magic> GetSpellList(int warriorID)
    {
        return warriorList[warriorID].MagicList;
    }

    public int GetWarriorLevel(int warriorID)
    {
        return warriorList[warriorID].WarriorLevel;
    }
    public void GiveDamageToNPC(int target, int warriorId)
    {
        if (!playersTurn)
        {
            // Prevent player attacks during enemies' turn
            Debug.Log("It's not your turn!");
            return;
        }
        for (int i = 0; i < EnemyCombatList.Count; i++)
        {
            if (EnemyCombatList[i].EnemyID == target)
            {
                EnemyCombatList[i].EnemyHP -= warriorList[warriorId].WarriorAttack;

                StartCoroutine(PlayAttackAnimation(warriorList[warriorId].WarriorGameObject.GetComponent<Animator>()));
                StartCoroutine(PlayHurtAnimation(EnemyCombatList[i].EnemyGameObject.GetComponent<Animator>()));
                
                EnemyCombatList[i].EnemyGameObject.GetComponent<EnemyTemplate>().TakeDamage(warriorList[warriorId].WarriorAttack);

                if (EnemyCombatList[i].EnemyHP <= 0)
                {
                    StartCoroutine(PlayDeathAnimation(EnemyCombatList[i].EnemyGameObject.GetComponent<Animator>()));
                    EnemyCombatList.RemoveAt(i);
                    UIManager.PopulateArrayPositions();
                    if (EnemyCombatList.Count != 0)
                    {
                        UIManager.MoveCrossair();
                    }
                }
                if (EnemyCombatList.Count == 0)
                {
                    UIManager.EndCombat();
                    EndScript();
                    break;
                }
                break;
            }
        }

        if (heroturncount >= warriorList.Count - 1)
        {
            playersTurn = false;
            StartCoroutine(SequenceEnemyAttacks());
            heroturncount = 0;
        }
        else
            heroturncount++;
    }
    public void GiveDamageToNPC(int target, int warriorId, int magicSelected)
    {
        if (!playersTurn)
        {
            // Prevent player attacks during enemies' turn
            Debug.Log("It's not your turn!");
            return;
        }
        for (int i = 0; i < EnemyCombatList.Count; i++)
        {
            if (EnemyCombatList[i].EnemyID == target)
            {
                if (warriorList[warriorId].WarriorMp >= warriorList[warriorId].MagicList[magicSelected].manaCost)
                {
                    EnemyCombatList[i].EnemyHP -= warriorList[warriorId].MagicList[magicSelected].damage;
                    warriorList[warriorId].WarriorMp -= warriorList[warriorId].MagicList[magicSelected].manaCost;

                    StartCoroutine(PlayAttackAnimation(warriorList[warriorId].WarriorGameObject.GetComponent<Animator>()));
                    StartCoroutine(PlayHurtAnimation(EnemyCombatList[i].EnemyGameObject.GetComponent<Animator>()));

                    EnemyCombatList[i].EnemyGameObject.GetComponent<EnemyTemplate>().TakeDamage(warriorList[warriorId].MagicList[magicSelected].damage);

                    if (EnemyCombatList[i].EnemyHP <= 0)
                    {
                        StartCoroutine(PlayDeathAnimation(EnemyCombatList[i].EnemyGameObject.GetComponent<Animator>()));
                        EnemyCombatList.RemoveAt(i);
                        UIManager.PopulateArrayPositions();
                        if (EnemyCombatList.Count != 0)
                        {
                            UIManager.MoveCrossair();
                        }
                    }
                    if (EnemyCombatList.Count == 0)
                    {
                        UIManager.EndCombat();
                        EndScript();
                        break;
                    }
                    break;
                } else
                {
                    Debug.Log("Not enough mana");
                }
            }
        }

        if (heroturncount >= warriorList.Count - 1)
        {
            playersTurn = false;
            StartCoroutine(SequenceEnemyAttacks());
            heroturncount = 0;
        }
        else
            heroturncount++;
    }
    private IEnumerator SequenceEnemyAttacks()
    {
        yield return new WaitForSeconds(1.0f);
        foreach (var enemy in EnemyCombatList)
        {
            // Get a random target hero
            int targetHero = UnityEngine.Random.Range(0, warriorList.Count);

            // Play the attack animation for the current enemy
            StartCoroutine(PlayAttackAnimation(enemy.EnemyGameObject.GetComponent<Animator>()));
            yield return new WaitForSeconds(0.5f);
            // Play the hurt animation for the targeted hero
            StartCoroutine(PlayHurtAnimation(warriorList[targetHero].WarriorGameObject.GetComponent<Animator>()));
            
            // Wait for the attack animation to finish
            yield return new WaitForSeconds(enemy.EnemyGameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);

            // Inflict damage on the targeted hero
            warriorList[targetHero].WarriorGameObject.GetComponent<HeroStats>().RecieveDamage(enemy.EnemyAtt);

            // Print damage information
            print("The hero " + warriorList[targetHero].WarriorName + " received " + enemy.EnemyAtt + " damage from Enemy " + enemy.EnemyName);

            // Update the hero's HP
            warriorList[targetHero].WarriorHP -= enemy.EnemyAtt;

            // Check if the hero is dead
            if (warriorList[targetHero].WarriorHP <= 0)
            {
                print("Your warrior is dead");
                if (warriorList.Count > 1)
                    warriorList.RemoveAt(targetHero);
                if (warriorList.Count == 1)
                {
                    UIManager.EndCombat();
                    EndScript();
                }
            }
        }
        playersTurn = true;
    }

    private void EndScript()
    {
        combatcanvasScript.DestroyChildren();
        inputManager.state = InputManager.ControllerState.Movable;
        EnemySpawner.KillSpawner();
        Invoke(nameof(ClearList), .2f);
        playersTurn = false;
        CombatPanel.SetActive(false);
    }

    void ClearList()
    {
        EnemyCombatList = new List<Enemy>();
    }

    private IEnumerator PlayAttackAnimation(Animator animator)
    {
        // Trigger attack animation
        animator.SetTrigger("Attack");

        // Wait for the duration of the attack animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
    }

    private IEnumerator PlayHurtAnimation(Animator animator)
    {
        // Trigger hurt animation
        animator.SetTrigger("Hurt");

        // Wait for the duration of the hurt animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
    }
    private IEnumerator PlayDeathAnimation(Animator animator)
    {
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
    }

}
