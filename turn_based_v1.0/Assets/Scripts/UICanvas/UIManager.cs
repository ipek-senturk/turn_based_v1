using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<Vector3> HeroPositions = new List<Vector3>();

    public PartyManager partymanager;
    private Vector3[] arrayPositions;
   // public GameObject UiHeroNames;
    public GameObject HeroSelector;
    public GameObject FirstOptionMenu;
    public GameObject crosshair;
    public GameObject OptionSelector;


    int crossairRotation = 0;
    int selectorRotation = 0;
    int selectionMenu = 0;
    int optionSelected;
    public int selectedWarriorId;
    bool playerSelect;
    bool optionSelect;
    bool targetSelect;

    public int targetID;

    private bool spaceReady;

    public void startUIManager()
    {
        HeroSelector.SetActive(true);
        populateArrayPostions();
        HeroCordinateList();
        gameObject.transform.position = arrayPositions[0];
        HeroSelector.transform.position = HeroPositions[0];
        OptionSelector.transform.position = FirstOptionMenu.transform.GetChild(0).GetChild(0).transform.position;
        OptionSelector.SetActive(false);
        crosshair.SetActive(false);
        optionSelected = 0;
        playerSelect = true;
        optionSelect = false;
        targetSelect = false;
        spaceReady = true;
    }

    private void Update()
    {

        if (targetSelect)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                moveCrossair();
            if (Input.GetKeyDown(KeyCode.Space) && spaceReady)
            {
                partymanager.GiveDamageToNPC(targetID, selectedWarriorId);
                targetSelect = false;
                playerSelect = true;


                clearHeroSelector();
                crosshair.SetActive(false);

                if (gameObject.activeInHierarchy)
                    StartCoroutine(SpaceWait());

                moveHeroSelector();
            }
        }

        if (playerSelect)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveHeroSelector();
            }
            if (Input.GetKeyDown(KeyCode.Space) && spaceReady)
            {
                HeroPositions.RemoveAt(selectorRotation);

                playerSelect = false;
                optionSelect = true;

                OptionSelector.SetActive(true);
                OptionSelector.transform.position = FirstOptionMenu.transform.GetChild(0).GetChild(0).transform.position;
                if (gameObject.activeInHierarchy)
                    StartCoroutine(SpaceWait());
            }
        }
        if (optionSelect)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                optionSelected = selectFirstOption();
            if (Input.GetKeyDown(KeyCode.Space) && spaceReady)
            {
                if (optionSelected == 0)
                {
                    optionSelect = false;

                    crosshair.SetActive(true);
                    targetSelect = true;
                    OptionSelector.SetActive(false);
                }

                if (gameObject.activeInHierarchy)
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

    private int selectFirstOption()
    {
        selectionMenu++;
        if (selectionMenu >= 3)
            selectionMenu = 0;

        OptionSelector.transform.position = FirstOptionMenu.transform.GetChild(0).GetChild(selectionMenu).transform.position;

        return selectionMenu;
    }

    private void moveHeroSelector()
    {
        selectorRotation++;
        if (selectorRotation >= HeroPositions.Count)
            selectorRotation = 0;

        HeroSelector.transform.position = HeroPositions[selectorRotation];
    }

    public void moveCrossair()
    {
        crossairRotation++;
        if (crossairRotation >= arrayPositions.Length)
            crossairRotation = 0;
        crosshair.transform.position = arrayPositions[crossairRotation];

    }

    private void clearHeroSelector()
    {
        if (HeroPositions.Count == 0)
        {
            targetID = 0;
            HeroCordinateList();
        }
        HeroSelector.transform.position = HeroPositions[0];
    }

    public void populateArrayPostions()
    {
        arrayPositions = new Vector3[partymanager.EnemyCombatList.Count];
        int i = 0;
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

    public void endCombat()
    {
        HeroSelector.gameObject.SetActive(false);
        StopCoroutine(SpaceWait());
        gameObject.SetActive(false);
    }
}
