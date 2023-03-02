using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("-----Player Stuff-----")]
    public GameObject player;
    public PlayerController playerController;
    public GameObject playerSpawnPosition;

    [Header("-----UI-----")]
    public bool isPaused;
    public GameObject reticle;
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;

    [Header("-----Game Goals-----")]
    public int enemiesRemaining;
    public int numberOfRounds;
    public int currentRound;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Position");
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && (activeMenu == null || activeMenu == pauseMenu))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                activeMenu = pauseMenu;
                activeMenu.SetActive(true);
                PauseState();
            }
            else UnpauseState();
        }
    }

    public void PauseState()
    {
        reticle.SetActive(false);

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnpauseState()
    {
        reticle.SetActive(true);
            
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void UpdateGameGoal(int value)
    {
        enemiesRemaining += value;

        if (enemiesRemaining <= 0) StartNextRound();
    }

    void StartNextRound()
    {
        currentRound++;

        if (currentRound > numberOfRounds)
        {
            PauseState();
            activeMenu = winMenu;
            activeMenu.SetActive(true);
        }
    }
}
