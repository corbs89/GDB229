using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
    public Slider hpSlider;
    public Slider staminaSlider;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI magazineText;
    public TextMeshProUGUI ammoReserveText;
    public GameObject weaponName;
    public TextMeshProUGUI weaponNameText;
    public float weaponNameTimer;
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;

    [Header("-----Game Goals-----")]
    public int enemiesRemaining;
    [Range(1f, 2f)] public float enemiesCoefficient = 1.5f;
    public int numberOfRounds; // 0 for infinite
    public int currentRound;
    public int roundTimer;
    public GameObject roundStartText;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Position");
    }

    void Start()
    {
        StartCoroutine(StartNextRound());
        hpSlider.maxValue = playerController.GetHP();
        hpSlider.value = playerController.GetHP();
        staminaSlider.maxValue = playerController.GetStamina();
        staminaSlider.value = staminaSlider.maxValue;
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

        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
            activeMenu = null;
        }
    }

    public void UpdateGameGoal(int value)
    {
        enemiesRemaining += value;

        if (enemiesRemaining <= 0)
        {
            if (currentRound >= numberOfRounds || numberOfRounds == 0)
            {
                PauseState();
                activeMenu = winMenu;
                activeMenu.SetActive(true);
                return;
            }

            StartCoroutine(StartNextRound());
        }
    }

    IEnumerator StartNextRound()
    {
        currentRound++;

        roundStartText.SetActive(true);
        roundStartText.GetComponent<TextMeshProUGUI>().text = "ROUND " + currentRound.ToString();

        enemiesRemaining = currentRound + (int)Mathf.Pow(enemiesCoefficient, currentRound + 1);

        yield return new WaitForSeconds(roundTimer);

        roundStartText.SetActive(false);
    }

    public void PlayerDead()
    {
        PauseState();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    public void UpdateMagazine(int magazine)
    {
        magazineText.text = magazine.ToString();
    }

    public void UpdateReserve(int reserve)
    {
        ammoReserveText.text = reserve.ToString();
    }

    public IEnumerator FlashWeaponName(Gun weapon)
    {
        weaponName.SetActive(true);
        weaponNameText.text = weapon.name; // GetName function required

        yield return new WaitForSeconds(weaponNameTimer);

        weaponName.SetActive(false);

    }
}
