using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public List<Vector3> HeroPositions = new List<Vector3>();

    public PartyManager partymanager;
    private Vector3[] arrayPositions;
    public GameObject HeroSelector;
    public GameObject FirstOptionMenu;
    public GameObject magicMenu;
    public GameObject crosshair;
    public GameObject OptionSelector;
    public GameObject magicSelector;
    public List<TextMeshProUGUI> spellNameTexts;

    int crossairRotation = 0;
    int selectorRotation = 0;
    int selectionMenu = 0;
    int magicSelectionMenu = 0;
    int optionSelected;
    int magicSelected;
    
    bool playerSelect;
    bool optionSelect;
    bool targetSelect;
    bool magicSelect;

    public int selectedWarriorId;
    public int targetID;

    private bool spaceReady;

    public void StartUIManager()
    {
        HeroSelector.SetActive(true);
        PopulateArrayPostions();
        HeroCordinateList();
        gameObject.transform.position = arrayPositions[0];
        HeroSelector.transform.position = HeroPositions[0];
        OptionSelector.transform.position = FirstOptionMenu.transform.GetChild(0).GetChild(0).transform.position;
        OptionSelector.SetActive(false);
        magicSelector.transform.position = magicMenu.transform.GetChild(0).transform.position;
        magicSelector.SetActive(false);
        crosshair.SetActive(false);
        optionSelected = 0;
        magicSelected = 0;
        playerSelect = true;
        optionSelect = false;
        targetSelect = false;
        magicSelect = false;
        spaceReady = true;
    }
    private void Update()
    {
        if (gameObject.activeInHierarchy && partymanager.GetIsPlayersTurn())
        {
            HandleTargetSelection();
            HandlePlayerSelection();
            HandleOptionSelection();
            HandleMagicSelection();
        }
    }
    private void HandleTargetSelection()
    {
        if (!gameObject.activeInHierarchy) return;
        if (targetSelect)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveCrossair();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && spaceReady)
            {
                partymanager.GiveDamageToNPC(targetID, selectedWarriorId);
                targetSelect = false;
                playerSelect = true;
                ClearHeroSelector();
                crosshair.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(SpaceWait());
                MoveHeroSelector();
            }
        }
    }

    private void HandlePlayerSelection()
    {
        if (!gameObject.activeInHierarchy) return;
        if (playerSelect)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveHeroSelector();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && spaceReady)
            {
                HeroPositions.RemoveAt(selectorRotation);
                playerSelect = false;
                optionSelect = true;
                OptionSelector.SetActive(true);
                OptionSelector.transform.position = FirstOptionMenu.transform.GetChild(0).GetChild(0).transform.position;
                StartCoroutine(SpaceWait());
            }
        }
    }

    private void HandleOptionSelection()
    {
        if (!gameObject.activeInHierarchy) return;
        if (optionSelect)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                optionSelected = SelectOption();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && spaceReady)
            {
                if (optionSelected == 0)
                {
                    optionSelect = false;
                    crosshair.SetActive(true);
                    targetSelect = true;
                    OptionSelector.SetActive(false);
                }
                else if (optionSelected == 1)
                {
                    optionSelect = false;
                    OptionSelector.SetActive(false);
                    magicSelect = true;
                    magicMenu.SetActive(true);
                    List<Magic> temp = partymanager.GetSpellList(selectedWarriorId);
                    int level = partymanager.GetWarriorLevel(selectedWarriorId);
                    for(int i = 0; i < level; i++)
                    {
                        spellNameTexts[i].text = temp[i].spellName;
                    }
                    magicSelector.SetActive(true);
                }
                StartCoroutine(SpaceWait());
            }
        }
    }

    private void HandleMagicSelection()
    {
        if (!gameObject.activeInHierarchy) return;
        if (magicSelect)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                magicSelected = SelectMagic();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && spaceReady)
            {
                if (magicSelected == 0)
                {
                    crosshair.SetActive(true);
                    targetSelect = true;
                    magicSelect = false;
                    magicMenu.SetActive(false);
                    magicSelector.SetActive(false);
                }
                StartCoroutine(SpaceWait());
            }
        }
    }
   
    private IEnumerator SpaceWait()
    {
        spaceReady = false;
        yield return new WaitForSeconds(0.2f);
        spaceReady = true;
    }

    private int SelectOption()
    {
        selectionMenu++;
        if (selectionMenu >= 3)
            selectionMenu = 0;
        OptionSelector.transform.position = FirstOptionMenu.transform.GetChild(0).GetChild(selectionMenu).transform.position;
        
        return selectionMenu;
    }

    private int SelectMagic()
    {
        magicSelectionMenu++;
        if(magicSelectionMenu >= 4)
            magicSelectionMenu = 0;
        magicSelector.transform.position = magicMenu.transform.GetChild(0).GetChild(magicSelectionMenu).transform.position; 
        
        return magicSelected;
    }


    private void MoveHeroSelector()
    {
        selectorRotation++;
        if (selectorRotation >= HeroPositions.Count)
            selectorRotation = 0;

        HeroSelector.transform.position = HeroPositions[selectorRotation];
    }

    public void MoveCrossair()
    {
        crossairRotation++;
        if (crossairRotation >= arrayPositions.Length)
            crossairRotation = 0;
        crosshair.transform.position = arrayPositions[crossairRotation];

    }

    private void ClearHeroSelector()
    {
        if (HeroPositions.Count == 0)
        {
            targetID = 0;
            HeroCordinateList();
        }
        HeroSelector.transform.position = HeroPositions[0];
    }

    public void PopulateArrayPostions()
    {
        int i = 0;
        arrayPositions = new Vector3[partymanager.EnemyCombatList.Count];
        foreach (var enemy in partymanager.EnemyCombatList)
        {
            arrayPositions[i] = enemy.position;
            i++;
        }
    }

    public void HeroCordinateList()
    {
        if (partymanager.warriorList.Count > 0)
        {
            HeroPositions = new List<Vector3>();

            foreach (var warrior in partymanager.warriorList)
            {
                HeroPositions.AddRange(new Vector3[]
                {
                    warrior.WarriorPosition
                });
            }
        }

    }

    public void EndCombat()
    {
        HeroSelector.SetActive(false);
        StopCoroutine(SpaceWait());
        gameObject.SetActive(false);
    }
}
