using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator characterAnimator;
    public Animator characterAnimator2;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }
    }

    public void PlayGame()
    {
        Debug.Log("Starting New Game");
        if (characterAnimator != null && characterAnimator2 != null)
        {
            characterAnimator.SetTrigger("Attack");
            characterAnimator2.SetTrigger("Attack");
        }
        GameManager.Instance.isNewGame = true; // Set flag to indicate a new game
        StartCoroutine(LoadScene("Level1"));
    }

    public void ContinueGame()
    {
        Debug.Log("Continuing Game");
        if (characterAnimator != null && characterAnimator2 != null)
        {
            characterAnimator.SetTrigger("Attack");
            characterAnimator2.SetTrigger("Attack");
        }

        GameManager.Instance.isNewGame = false; // Set flag to indicate a saved game

        if (SaveSystem.SaveExists())
        {
            GameData data = SaveSystem.LoadGame();
            StartCoroutine(LoadScene(data.currentScene));
        }
        else
        {
            // Handle case where no save exists
            Debug.Log("No saved game found!");
            StartCoroutine(LoadScene("Level1")); // Fall back to starting a new game
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        if (characterAnimator != null && characterAnimator2 != null)
        {
            characterAnimator.SetTrigger("Attack");
            characterAnimator2.SetTrigger("Attack");
        }

        Application.Quit();
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}
