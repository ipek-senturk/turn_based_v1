using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator characterAnimator;
    public Animator characterAnimator2;
    private bool isNewGame = true; // Flag to track whether it's a new game or a saved game
    public static MainMenu Instance;

    private void Start()
    {
        Instance = this;
    }
    public void PlayGame()
    {
        if (characterAnimator != null && characterAnimator2 != null)
        {
            characterAnimator.SetTrigger("Attack");
            characterAnimator2.SetTrigger("Attack");
        }
        isNewGame = true; // Set flag to indicate a new game
        StartCoroutine(LoadScene("Level1"));
    }

    public void ContinueGame()
    {
        if (characterAnimator != null && characterAnimator2 != null)
        {
            characterAnimator.SetTrigger("Attack");
            characterAnimator2.SetTrigger("Attack");
        }

        isNewGame = false; // Set flag to indicate a saved game

        if (PlayerPrefs.HasKey("SavedScene"))
        {
            string sceneName = PlayerPrefs.GetString("SavedScene");      
            StartCoroutine(LoadScene(sceneName));
        }
        else
        {
            // Handle case where no save exists
            Debug.Log("No saved game found!");
            StartCoroutine(LoadScene("Level1"));
        }
    }

    public void QuitGame()
    {
        if(characterAnimator != null && characterAnimator2 != null)
        {
            characterAnimator.SetTrigger("Attack");
            characterAnimator2.SetTrigger("Attack");
        }

        Application.Quit();
        
    }

    public IEnumerator LoadScene(string name)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }

    public bool IsNewGame()
    {
        return isNewGame;
    }
}

