using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Resume()
    {
        GameManager.instance.UnpauseState();
        GameManager.instance.isPaused = !GameManager.instance.isPaused;
    }

    public void Restart()
    {
        GameManager.instance.UnpauseState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("This quits the game.");
    }

    public void MainMenu()
    {
        Debug.Log("This goes to the Main Menu.");
    }

    public void PlayerRespawn()
    {
        Resume();
        GameManager.instance.playerController.SpawnPlayer();
    }

    public void ReduceEnemiesRemaining()
    {
        Resume();
        GameManager.instance.UpdateGameGoal(-100);
    }

    public void ReduceHP()
    {
        Resume();
        GameManager.instance.playerController.TakeDamage(5);
    }
    public void AddPoints()
    {
        GameManager.instance.playerController.IncrementPoints(100);
    }
    public void ShakeCamera()
    {
        Resume();
        CameraControls.instance.CameraShake(30, 0.5f);
    }
    public static void LoadLevel(int sceneNumber)
    {
        Debug.Log("Loading scene");
        switch(sceneNumber) 
        {
            case 0:
                SceneManager.LoadScene("Armory");
                break;
            case 1:
                Debug.Log("loading 1");
                SceneManager.LoadScene("Map 1");
                break;
            case 2:
                SceneManager.LoadScene("Map 2");
                break;
            case 3:
                SceneManager.LoadScene("Map 3");
                break;
            case 4:
                SceneManager.LoadScene("Map 4");
                break;

        }
    }
}
