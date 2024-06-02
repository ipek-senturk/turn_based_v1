using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour
{
    private bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isActive)
            {
                CloseMenu();
            } 
            else
            {
                OpenMenu();
            }
        }
    }

    public void OpenMenu()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        isActive = true;
    }

    public void CloseMenu() 
    {
        transform.GetChild(0).gameObject.SetActive(false);
        isActive = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
