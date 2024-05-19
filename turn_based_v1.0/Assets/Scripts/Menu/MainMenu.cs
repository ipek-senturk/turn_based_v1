using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator characterAnimator;
    public Animator characterAnimator2;

    public void PlayGame()
    {
        if (characterAnimator != null && characterAnimator2 != null)
        {
            characterAnimator.SetTrigger("Attack");
            characterAnimator2.SetTrigger("Attack");
        }
    }

    public void ContinueGame()
    {
        if (characterAnimator != null && characterAnimator2 != null)
        {
            characterAnimator.SetTrigger("Attack");
            characterAnimator2.SetTrigger("Attack");
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

    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

