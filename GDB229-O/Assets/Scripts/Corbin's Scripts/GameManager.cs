using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Image hpFill;
    public Image staminaFill;
    public Image reloadMeter;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI magazineText;
    public TextMeshProUGUI ammoReserveText;
    public GameObject reserveWarning;
    public GameObject weaponName;
    public TextMeshProUGUI weaponNameText;
    public float weaponNameTimer;
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject screenFlash;
    public float screenFlashTimer;

    [Header("-----Game Goals-----")]
    public int enemiesRemaining;
    public int enemiesSpawned;
    public int enemiesMax;
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


    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Armory") StartCoroutine(StartNextRound());
        reloadMeter.fillAmount = 0f;

        

        if (playerController.equippedWeapon != null)
        {
            playerController.equippedWeapon.UpdateUI();
            playerController.equippedWeapon.refillAmmo();
        }

        playerSpawnPosition = GameObject.FindGameObjectWithTag("Player Spawn Position");
        Debug.Log(playerController);
        playerController.SpawnPlayer();
        //SpawnManager.instance.TriggerSpawn();
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

    public bool CanSpawnEnemy()
    {
        if (enemiesSpawned < enemiesMax)
        {
            enemiesSpawned++;
            return true;
        }

        return false;
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
        enemiesSpawned = 0;
        
        roundStartText.SetActive(true);
        roundStartText.GetComponent<TextMeshProUGUI>().text = "ROUND " + currentRound.ToString();

        enemiesMax = currentRound + (int)Mathf.Pow(enemiesCoefficient, currentRound + 1);
        enemiesRemaining = enemiesMax;

        yield return new WaitForSeconds(roundTimer);
        SpawnManager.instance.TriggerSpawn();
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
    public IEnumerator FlashReservewarning()
    {
        reserveWarning.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        reserveWarning.SetActive(false);
    }

    public IEnumerator StartReloadMeter()
    {
        float timeSpentReloading = 0f;
        float reloadSpeed = playerController.equippedWeapon.GetReloadSpeed();
        playerController.ToggleCanSwitchWeapon(false);

        while (timeSpentReloading < reloadSpeed)
        {
            timeSpentReloading += Time.deltaTime;
            reloadMeter.fillAmount = timeSpentReloading / reloadSpeed;

            yield return new WaitForEndOfFrame();
        }

        reloadMeter.fillAmount = 0f;
        playerController.ToggleCanSwitchWeapon(true);
    }

    public IEnumerator playerHit(float flashTimer, float shakeTimer)
    {
        CameraControls.instance.CameraShake(50, 0.0675f);
        CameraControls.instance.CameraShake(50, shakeTimer);
        screenFlash.SetActive(true);
        yield return new WaitForSeconds(screenFlashTimer);
        yield return new WaitForSeconds(flashTimer);
        screenFlash.SetActive(false);
    }


}
