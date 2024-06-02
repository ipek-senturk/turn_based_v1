using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int sceneBuildIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Scene trigger entered");
        if (collision.CompareTag("Player"))
        {
            print("Switching scene to " + sceneBuildIndex);
            GameManager.Instance.SaveGameState(); // Save the game state before switching scenes
            GameManager.Instance.isTransitioning = true; // Set transition flag
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}
