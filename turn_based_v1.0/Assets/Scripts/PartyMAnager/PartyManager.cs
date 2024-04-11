using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public List<Warrior> warriorList = new List<Warrior>();
    public List<Enemy> EnemyCombatList = new List<Enemy>();

    public UIManager UiManager;
    public InputManager inputManager;

    public CombatCanvas combatcanvasScript;
    public Input playerInput;
    public spawner EnemySpawner;
    public GameObject CombatPanel;
    public HeroStats heroStats;

    private int heroturncount = 0;

    public void addWarriorToList(HeroStats.WarriorData warriorData, Vector3 position, GameObject warriorGameObject)
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
            WarriorId = warriorList.Count
        });
    }

    
    public void addEnemyToCombatList(EnemyTemplate.EnemyData enemyData, Vector3 pos, GameObject gameObject)
    {
        EnemyCombatList.Add(new Enemy
        {
            EnemyName = enemyData.Name,
            EnemyHP = enemyData.HP,
            EnemyID = EnemyCombatList.Count,
            position = pos,
            EnemygameObject = gameObject,
            EnemyAtt = enemyData.Att
        });
    }
    public void startCombat()
    {
        UiManager.gameObject.SetActive(true);
        UiManager.startUIManager();
    }


    public void getHeroStats()
    {
        foreach (var warrior in warriorList)
        {
            warrior.WarriorPosition = warrior.WarriorGameObject.GetComponent<Transform>().position;
        }
    }

    public void GiveDamageToNPC(int target, int warriorId)
    {
        for (int i = 0; i < EnemyCombatList.Count; i++)
        {
            if (EnemyCombatList[i].EnemyID == target)
            {
                EnemyCombatList[i].EnemyHP -= warriorList[warriorId].WarriorAttack;
                EnemyCombatList[i].EnemygameObject.GetComponent<EnemyTemplate>().TakeDamage(warriorList[warriorId].WarriorAttack);
                if (EnemyCombatList[i].EnemyHP <= 0)
                {
                    EnemyCombatList.RemoveAt(i);
                    UiManager.populateArrayPostions();
                    if (EnemyCombatList.Count != 0)
                    {
                        UiManager.moveCrossair();
                    }
                }
                if (EnemyCombatList.Count == 0)
                {
                    endScript();
                    break;
                }
                break;
            }
        }

        if (heroturncount >= warriorList.Count - 1)
        {
            GiveDamageToPlayer();
            heroturncount = 0;
        }
        else
            heroturncount++;
    }

    public void GiveDamageToPlayer()
    {
        int targetHero;
        foreach (var enemy in EnemyCombatList)
        {
            targetHero = UnityEngine.Random.Range(0, warriorList.Count);
            warriorList[targetHero].WarriorGameObject.GetComponent<HeroStats>().RecieveDamage(enemy.EnemyAtt);
            print("The hero " + warriorList[targetHero].WarriorName + "  Recieved " + enemy.EnemyAtt + " dmage from Enemy " + enemy.EnemyName);
            warriorList[targetHero].WarriorHP -= enemy.EnemyAtt;

            if (warriorList[targetHero].WarriorHP <= 0)
            {
                print("your warrior is dead");
                if (warriorList.Count > 1)
                    warriorList.RemoveAt(targetHero);
                if (warriorList.Count == 1)
                {
                    UiManager.endCombat();
                    endScript();
                }
            }
        }
    }

    private void endScript()
    {
        combatcanvasScript.destroychildrens();
        inputManager.state = InputManager.ControllerState.Movable;
        EnemySpawner.killSpawner();
        Invoke("clearLists", .2f);

        CombatPanel.gameObject.SetActive(false);
    }

    void clearLists()
    {
        EnemyCombatList = new List<Enemy>();
    }
}
