﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<Vector3> HeroPositions = new List<Vector3>();
    public PartyManager partymanager;
    private Vector3[] arrayPositions;
    public GameObject HeroSelector;
    public GameObject FirstOptionMenu;
    public GameObject magicMenu;
    public GameObject crosshair;

    int crossairRotation = 0;
    int selectorRotation = 0;
    int selectionMenu = 0;
    int selectedSpellIndex = 0;

    bool playerSelect;
    bool targetSelect;
    public bool spellSelected;

    public int selectedWarriorId;
    public int targetID;

    private bool enterReady;
    public void StartUIManager()
    {
        HeroSelector.SetActive(true);
        PopulateArrayPositions();
        HeroPositionsList();

        if (arrayPositions.Length == 0)
        {
            Debug.LogError("No enemies found to initialize UIManager.");
            return;
        }

        // Deactivate buttons in the beginning
        SetOptionButtonsInteractable(false);

        gameObject.transform.position = arrayPositions[0];
        HeroSelector.transform.position = HeroPositions[0];
        crosshair.SetActive(false);
        spellSelected = false;
        playerSelect = true;
        targetSelect = false;
        enterReady = true;
    }

    private void Update()
    {
        if (partymanager.GetIsPlayersTurn())
        {
            HandlePlayerSelection();
            HandleOptionSelection();
            HandleTargetSelection();
        }
    }

    private void HandlePlayerSelection()
    {
        if (playerSelect)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveHeroSelector();
            }
            else if (Input.GetKeyDown(KeyCode.E) && enterReady)
            {
                HeroPositions.RemoveAt(selectorRotation);
                playerSelect = false;
                SelectFirstButtonInMenu();
                // Make buttons interactable after hero is chosen
                SetOptionButtonsInteractable(true);
                StartCoroutine(EnterWait());
            }
        }
    }

    private void HandleOptionSelection()
    {
        if (!playerSelect) // Ensure player is selected
        {
            if (Input.GetKeyDown(KeyCode.Return) && enterReady)
            {
                // Handle button click
                switch (selectionMenu)
                {
                    case 0:
                        OnAttackButton();
                        break;
                    case 1:
                        OnSkillsButton();
                        break;
                    case 2:
                        OnItemsButton();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private void HandleTargetSelection()
    {
        if (targetSelect)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveCrossair();
            }
            else if (Input.GetKeyDown(KeyCode.E) && enterReady)
            {
                if (spellSelected) // Check if it's magic attack
                {
                    Debug.Log("Spell damage or effects applied to NPC");
                    if(partymanager.GetWarriorMana(selectedWarriorId) >= partymanager.GetManaCost(selectedWarriorId, selectedSpellIndex))
                    {
                        partymanager.GiveDamageToNPC(targetID, selectedWarriorId, selectedSpellIndex);
                    } else
                    {
                        Debug.Log("Not enough mana");
                    }
                }
                else
                {
                    Debug.Log("Physical damage");
                    partymanager.GiveDamageToNPC(targetID, selectedWarriorId);
                }
                targetSelect = false;
                playerSelect = true;
                ClearHeroSelector();
                crosshair.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(EnterWait());
                MoveHeroSelector();
            }
        }
    }
    private IEnumerator EnterWait()
    {
        enterReady = false;
        yield return new WaitForSeconds(0.2f);
        enterReady = true;
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
            HeroPositionsList();
        }
        HeroSelector.transform.position = HeroPositions[0];
    }
    public void PopulateArrayPositions()
    {
        int i = 0;
        arrayPositions = new Vector3[partymanager.EnemyCombatList.Count];
        foreach (var enemy in partymanager.EnemyCombatList)
        {
            arrayPositions[i] = enemy.position;
            i++;
        }
    }
    public void HeroPositionsList()
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
    public void OnAttackButton()
    {
        spellSelected = false;
        SetOptionButtonsInteractable(false);
        crosshair.SetActive(true);
        targetSelect = true;
    }
    public void OnSkillsButton()
    {
        spellSelected = true;
        magicMenu.SetActive(true);
        SetMagicButtonsInteractable(false);
        List<Magic> temp = partymanager.GetSpellList(selectedWarriorId);
        int level = partymanager.GetWarriorLevel(selectedWarriorId);
        for (int i = 0; i < level; i++)
        {
            magicMenu.transform.GetChild(0).transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = temp[i].spellName;
            magicMenu.transform.GetChild(0).transform.GetChild(i).GetComponent<Button>().interactable = true; 
        }
        Debug.Log("Magic Select");
    }
    public void OnSpellButtonClick(int spellIndex)
    {
        spellSelected = true;
        SetOptionButtonsInteractable(false);
        magicMenu.SetActive(false);
        selectedSpellIndex = spellIndex;
        if(spellSelected)
        {
            Debug.Log("Spell " + spellIndex + " selected!");
        }
        crosshair.SetActive(true);
        targetSelect = true;
    }
    public void OnItemsButton()
    {
        Debug.Log("Item select");
    }
    public void EndCombat()
    {
        HeroSelector.SetActive(false);
        StopCoroutine(EnterWait());
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }
    private void SelectFirstButtonInMenu()
    {
        // Select Attack button
        Transform firstButton = FirstOptionMenu.transform.GetChild(0).transform.GetChild(0);
        if (firstButton != null)
        {
            Button buttonComponent = firstButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.Select();
            }
        }
    }
    private void SetOptionButtonsInteractable(bool interactable)
    {
        // Set interactable status for option buttons
        int i , len = 3;
        for (i = 0; i < len; i++)
        {
            FirstOptionMenu.transform.GetChild(0).transform.GetChild(i).GetComponent<Button>().interactable = interactable;
        }
    }
    private void SetMagicButtonsInteractable(bool interactable)
    {
        int i, len = 4;
        for (i = 0; i < len; i++)
        {
            magicMenu.transform.GetChild(0).transform.GetChild(i).GetComponent<Button>().interactable = interactable;
        }
    }
}
