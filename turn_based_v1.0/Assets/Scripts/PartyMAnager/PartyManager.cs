﻿using System;
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
            MagicList = warriorData.magicList,
            Inventory = warriorData.inventory
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
    public int GetWarriorMana(int warriorID)
    {
        return warriorList[warriorID].WarriorMp;
    }
    public int GetManaCost(int warriorID, int magicSelected)
    {
        return warriorList[warriorID].MagicList[magicSelected].manaCost;
    }
    public int FindMageInList()
    {
        int index = 0;
        for (int i = 0; i < warriorList.Count; i++)
        {
            if (warriorList[i].WarriorName == "Mage")
            {
                index = i;
                break;
            }
        }
        return index;
    }
    public Inventory GetInventory()
    {
        return warriorList[FindMageInList()].Inventory;
    }
    public void GiveDamageToNPC(int target, int warriorId, int magicSelected = -1)
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
                int damage;
                if (magicSelected >= 0)
                {
                    // Magic attack
                    damage = warriorList[warriorId].MagicList[magicSelected].damage;
                    int manaCost = warriorList[warriorId].MagicList[magicSelected].manaCost;
                    warriorList[warriorId].WarriorMp -= manaCost; // Deduct mana cost
                    warriorList[warriorId].WarriorGameObject.GetComponent<HeroStats>().CastSpell(manaCost); // Call CastSpell method
                }
                else
                {
                    // Physical attack
                    damage = warriorList[warriorId].WarriorAttack;
                }

                EnemyCombatList[i].EnemyHP -= damage;

                StartCoroutine(PlayAttackAnimation(warriorList[warriorId].WarriorGameObject.GetComponent<Animator>()));
                StartCoroutine(PlayHurtAnimation(EnemyCombatList[i].EnemyGameObject.GetComponent<Animator>()));

                EnemyCombatList[i].EnemyGameObject.GetComponent<EnemyTemplate>().TakeDamage(damage);

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
        {
            heroturncount++;
        }
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
    public void UseItem(int warriorID, Item.ItemType itemType)
    {
        if (!playersTurn)
        {
            // Prevent player actions during enemies' turn
            Debug.Log("It's not your turn!");
            return;
        }

        List<Item> temp = GetInventory().GetItemList();
        int itemIndex = GetInventory().GetItemIndex(itemType);

        if (itemIndex == -1 || temp[itemIndex].amount <= 0)
        {
            Debug.Log("No item");
            return;
        }

        int hpPoints = 50;
        int mpPoints = 10;

        if (itemType == Item.ItemType.HealthPotion)
        {
            // HP Potion
            warriorList[warriorID].WarriorHP += hpPoints;
            warriorList[warriorID].WarriorGameObject.GetComponent<HeroStats>().RecieveDamage(-hpPoints);
            warriorList[FindMageInList()].WarriorGameObject.GetComponent<HeroStats>().UseItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
        }
        else if (itemType == Item.ItemType.ManaPotion)
        {
            // MP Potion
            warriorList[warriorID].WarriorMp += mpPoints;
            warriorList[warriorID].WarriorGameObject.GetComponent<HeroStats>().CastSpell(-mpPoints);
            warriorList[FindMageInList()].WarriorGameObject.GetComponent<HeroStats>().UseItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
        }

        // Decrease the item amount in the inventory
        // temp[itemIndex].amount--;
        // if (temp[itemIndex].amount <= 0)
        // {
        //     temp.RemoveAt(itemIndex); // Remove the item if amount is zero
        // }

        if (heroturncount >= warriorList.Count - 1)
        {
            playersTurn = false;
            StartCoroutine(SequenceEnemyAttacks());
            heroturncount = 0;
        }
        else
        {
            heroturncount++;
        }
    }
    private void EndScript()
    {
        combatcanvasScript.DestroyChildren();
        inputManager.state = InputManager.ControllerState.Movable;
        EnemySpawner.KillSpawner();
        Invoke(nameof(ClearList), 0.2f);
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
