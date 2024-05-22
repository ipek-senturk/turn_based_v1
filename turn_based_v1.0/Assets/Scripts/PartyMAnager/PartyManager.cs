using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public List<Warrior> warriorList = new List<Warrior>();
    public List<Enemy> EnemyCombatList = new List<Enemy>();

    public UIManager UIManager;
    public InputManager inputManager;

    public CombatCanvas combatcanvasScript;
    public Input playerInput;
    public Spawner EnemySpawner;
    public GameObject CombatPanel;
    public HeroStats heroStats;

    private int heroturncount = 0;
    private bool playersTurn = true;
    private Dictionary<int, int> heroIdToIndexMap = new Dictionary<int, int>();

    public void AddWarriorToList(HeroStats.WarriorData warriorData, Vector3 position, GameObject warriorGameObject)
    {
        var warrior = new Warrior
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
        };
        warriorList.Add(warrior);
        heroIdToIndexMap[warrior.WarriorId] = warriorList.Count - 1; // Map ID to the current index
        Debug.Log($"Added Warrior: {warrior.WarriorName}, ID: {warrior.WarriorId}, Index: {warriorList.Count - 1}");
    }
    public void RemoveWarrior(int warriorIndex)
    {
        if (warriorIndex < 0 || warriorIndex >= warriorList.Count)
        {
            Debug.LogWarning($"Invalid Warrior Index: {warriorIndex}");
            return; // Invalid index
        }

        int warriorId = warriorList[warriorIndex].WarriorId;

        Debug.Log($"Removing Warrior: {warriorList[warriorIndex].WarriorName}, ID: {warriorId}, Index: {warriorIndex}");
        warriorList.RemoveAt(warriorIndex);
        heroIdToIndexMap.Remove(warriorId);
        UpdateHeroIdToIndexMap(); // Update the map after removal
    }
    private void UpdateHeroIdToIndexMap()
    {
        for (int i = 0; i < warriorList.Count; i++)
        {
            heroIdToIndexMap[warriorList[i].WarriorId] = i;
            Debug.Log($"Updated Warrior ID: {warriorList[i].WarriorId} to Index: {i}");
        }
    }

    public int GetWarriorIndexById(int warriorId)
    {
        return heroIdToIndexMap.TryGetValue(warriorId, out int index) ? index : -1;
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
    public List<Magic> GetSpellList(int warriorId)
    {
        int warriorIndex = GetWarriorIndexById(warriorId);
        // if (warriorIndex == -1) return; // Invalid ID
        return warriorList[warriorIndex].MagicList;
    }

    public int GetWarriorLevel(int warriorId)
    {
        int warriorIndex = GetWarriorIndexById(warriorId);
        if (warriorIndex == -1) return -1; // Invalid ID
        return warriorList[warriorIndex].WarriorLevel;
    }
    public int GetWarriorMana(int warriorId)
    {
        int warriorIndex = GetWarriorIndexById(warriorId);
        // if (warriorIndex == -1) return; // Invalid ID
        return warriorList[warriorIndex].WarriorMp;
    }
    public int GetManaCost(int warriorId, int magicSelected)
    {
        int warriorIndex = GetWarriorIndexById(warriorId);
        if (warriorIndex == -1) return -1; // Invalid ID
        return warriorList[warriorIndex].MagicList[magicSelected].manaCost;
    }
    public int FindMageInList()
    {
        int index = -1;
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
        if(FindMageInList() != -1)
            return warriorList[FindMageInList()].Inventory;
        // Return an empty inventory here
        else return null;
    }
    public void GiveDamageToNPC(int target, int warriorId, int magicSelected = -1)
    {
        if (!playersTurn)
        {
            Debug.Log("It's not your turn!");
            return;
        }

        int warriorIndex = GetWarriorIndexById(warriorId);
        if (warriorIndex == -1)
        {
            Debug.LogWarning($"Invalid Warrior ID: {warriorId}");
            return; // Invalid ID
        }

        Debug.Log($"Warrior {warriorList[warriorIndex].WarriorName} with ID: {warriorId} at Index: {warriorIndex} attacks.");

        for (int i = 0; i < EnemyCombatList.Count; i++)
        {
            if (EnemyCombatList[i].EnemyID == target)
            {
                int damage;
                if (magicSelected >= 0)
                {
                    // Magic attack
                    damage = warriorList[warriorIndex].MagicList[magicSelected].damage;
                    int manaCost = warriorList[warriorIndex].MagicList[magicSelected].manaCost;
                    warriorList[warriorIndex].WarriorMp -= manaCost;
                    warriorList[warriorIndex].WarriorGameObject.GetComponent<HeroStats>().CastSpell(manaCost);
                    Debug.Log($"Warrior {warriorList[warriorIndex].WarriorName} casts spell with damage: {damage} and mana cost: {manaCost}");
                }
                else
                {
                    // Physical attack
                    damage = warriorList[warriorIndex].WarriorAttack;
                    Debug.Log($"Warrior {warriorList[warriorIndex].WarriorName} performs physical attack with damage: {damage}");
                }

                EnemyCombatList[i].EnemyHP -= damage;
                Debug.Log($"Enemy {EnemyCombatList[i].EnemyName} takes {damage} damage. Remaining HP: {EnemyCombatList[i].EnemyHP}");

                StartCoroutine(PlayAttackAnimation(warriorList[warriorIndex].WarriorGameObject.GetComponent<Animator>()));
                StartCoroutine(PlayHurtAnimation(EnemyCombatList[i].EnemyGameObject.GetComponent<Animator>()));

                EnemyCombatList[i].EnemyGameObject.GetComponent<EnemyTemplate>().TakeDamage(damage);

                if (EnemyCombatList[i].EnemyHP <= 0)
                {
                    Debug.Log($"Enemy {EnemyCombatList[i].EnemyName} defeated!");
                    // StartCoroutine(PlayDeathAnimation(EnemyCombatList[i].EnemyGameObject.GetComponent<Animator>()));
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
            warriorList[targetHero].WarriorGameObject.GetComponent<HeroStats>().ReceiveDamage(enemy.EnemyAtt);

            // Print damage information
            print("The hero " + warriorList[targetHero].WarriorName + " received " + enemy.EnemyAtt + " damage from Enemy " + enemy.EnemyName);

            // Update the hero's HP
            warriorList[targetHero].WarriorHP -= enemy.EnemyAtt;

            // Check if the hero is dead
            if (warriorList[targetHero].WarriorHP <= 0)
            {
                print("Your warrior is dead");

                // Check if the dead warrior is the mage (main player)
                if (targetHero == FindMageInList())
                {
                    // Ensure mage's HP doesn't drop below 1
                    warriorList[targetHero].WarriorHP = 1;
                    // Do not remove the mage from the list, just continue
                    Debug.Log("Mage's HP set to 1, not removing from the list");
                    continue;
                }
                else
                {
                    // Remove the non-mage warrior if the list has more than one warrior
                    if (warriorList.Count > 1)
                    {
                        // Sending the index directly
                        RemoveWarrior(targetHero);
                        UIManager.HeroPositionsList();
                    }

                    // Check if only the mage remains in the list
                    if (warriorList.Count == 1)
                    {
                        UIManager.EndCombat();
                        EndScript();
                        yield break;
                    }
                }
            }
        }
        playersTurn = true;
    }

    public void UseItem(int warriorId, Item.ItemType itemType)
    {
        if (!playersTurn)
        {
            // Prevent player actions during enemies' turn
            Debug.Log("It's not your turn!");
            return;
        }

        int warriorIndex = GetWarriorIndexById(warriorId);
        if (warriorIndex == -1) return; // Invalid ID

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
            warriorList[warriorIndex].WarriorHP += hpPoints;
            warriorList[warriorIndex].WarriorGameObject.GetComponent<HeroStats>().ReceiveDamage(-hpPoints);
            warriorList[FindMageInList()].WarriorGameObject.GetComponent<HeroStats>().UseItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
        }
        else if (itemType == Item.ItemType.ManaPotion)
        {
            // MP Potion
            warriorList[warriorIndex].WarriorMp += mpPoints;
            warriorList[warriorIndex].WarriorGameObject.GetComponent<HeroStats>().CastSpell(-mpPoints);
            warriorList[FindMageInList()].WarriorGameObject.GetComponent<HeroStats>().UseItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });
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
