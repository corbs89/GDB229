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

    public void ReduceHPToZero()
    {
        Resume();
        GameManager.instance.playerController.TakeDamage(100);
    }
}
