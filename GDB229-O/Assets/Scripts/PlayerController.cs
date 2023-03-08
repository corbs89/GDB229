using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController characterController;

    [Header("-----Objects-----")]
    [SerializeField] GameObject gunPosition;

    [Header("-----Player Stats-----")]
    [SerializeField] int HP;
    [SerializeField] float stamina;
    [SerializeField][Range(1f, 10f)] float playerSpeed;
    [SerializeField][Range(1f, 2f)] float sprintCoefficient;
    [SerializeField][Range(0, 5)] int sprintCost;
    [SerializeField][Range(1f, 10f)] float staminaRechargeRate;
    [SerializeField][Range(1f, 10f)] float staminaRechargeStartTime;
    [SerializeField][Range(1, 5)] int jumpTimes;
    [SerializeField][Range(5, 50)] int jumpSpeed;
    [SerializeField][Range(10, 75)] int gravity;



    [Header("-----Gun Stats-----")]
    [SerializeField] GameObject weaponSlot1Object;
    [SerializeField] GameObject weaponSlot2Object;
    [SerializeField][Range(0f, 10f)] float weightModifier;


    int originalHP;
    float staminaMax;
    float timeSinceUsedStamina = Mathf.Infinity;
    int jumpsCurrent;
    Vector3 movement;
    Vector3 playerVelocity;
    int points;
    public bool canSwitchWeapon = true;


    Gun weaponSlot1;
    Gun weaponSlot2;
    Renderer slot1Renderer;
    Renderer slot2Renderer;
    public Gun equippedWeapon;

    public int GetHP() { return HP; }
    public float GetStamina() { return stamina; }
    public int GetPoints() { return points; }

    void Start()
    {
        originalHP = HP;
        staminaMax = stamina;
        UpdateHPUI();
        SpawnPlayer();

        EquipWeaponInSlot2(weaponSlot2Object);
        EquipWeaponInSlot1(weaponSlot1Object);

        weaponSlot2.enabled = false;
        slot2Renderer.enabled = false;
    }


    void Update()
    {
        ResetJump();
        ProcessMovement();
        ProcessJump();
        IncrementStamina();
        SwapWeapons();

        timeSinceUsedStamina += Time.deltaTime;
    }

    void ResetJump()
    {
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            jumpsCurrent = 0;
            playerVelocity.y = 0;
        }
    }

    void ProcessMovement()
    {
        movement = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        movement = Vector3.Normalize(movement);

        float currentSpeed;

        if (equippedWeapon != null)
        {
            currentSpeed = playerSpeed - (playerSpeed / (100 - equippedWeapon.GetWeight()) * weightModifier);
        }
        else currentSpeed = playerSpeed;

        if (IsSprinting() && stamina != 0 && movement != Vector3.zero)
        {
            currentSpeed *= sprintCoefficient;
            DecrementStamina();
        }

        characterController.Move(currentSpeed * Time.deltaTime * movement);
    }

    bool IsSprinting()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    void IncrementStamina()
    {
        if (timeSinceUsedStamina > staminaRechargeStartTime)
        {
            stamina += staminaRechargeRate * Time.deltaTime;

            stamina = Mathf.Clamp(stamina, 0f, staminaMax);

            if (stamina == staminaMax) ToggleStaminaPie(false);
        }

        UpdateStaminaUI();
    }

    void DecrementStamina()
    {
        ToggleStaminaPie(true);
        timeSinceUsedStamina = 0;
        stamina -= sprintCost * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0f, staminaMax);
        UpdateStaminaUI();
    }

    void ProcessJump()
    {
        if (Input.GetButtonDown("Jump") && jumpsCurrent < jumpTimes)
        {
            playerVelocity.y = jumpSpeed;
            jumpsCurrent++;
        }

        playerVelocity.y -= gravity * Time.deltaTime;

        characterController.Move(Time.deltaTime * playerVelocity);
    }

    void EquipWeaponInSlot1(GameObject newWeapon)
    {
        weaponSlot1Object = newWeapon;

        weaponSlot1 = Instantiate(weaponSlot1Object.transform.GetComponentInChildren<Gun>(), gunPosition.transform);

        slot1Renderer = weaponSlot1.GetComponent<Renderer>();

        weaponSlot1.transform.localPosition = Vector3.zero;

        string stringToReplace = "(Clone)";
        weaponSlot1.transform.name = weaponSlot1.transform.name.Replace(stringToReplace, "");

        equippedWeapon = weaponSlot1;

        equippedWeapon.enabled = true;
    }

    void EquipWeaponInSlot2(GameObject newWeapon)
    {
        weaponSlot2Object = newWeapon;

        weaponSlot2 = Instantiate(weaponSlot2Object.transform.GetComponentInChildren<Gun>(), gunPosition.transform);

        slot2Renderer = weaponSlot2.GetComponent<Renderer>();

        weaponSlot2.transform.localPosition = Vector3.zero;

        string stringToReplace = "(Clone)";
        weaponSlot2.transform.name = weaponSlot2.transform.name.Replace(stringToReplace, "");

        equippedWeapon = weaponSlot2;
        equippedWeapon.enabled = true;
    }


    void SwapWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canSwitchWeapon)
        {
            equippedWeapon.enabled = false;

            if (equippedWeapon == weaponSlot1)
            {
                slot1Renderer.enabled = false;
                slot2Renderer.enabled = true;
                equippedWeapon = weaponSlot2;
            }
            else if (equippedWeapon == weaponSlot2)
            {
                slot1Renderer.enabled = true;
                slot2Renderer.enabled = false;
                equippedWeapon = weaponSlot1;
            }

            equippedWeapon.enabled = true;
            equippedWeapon.UpdateUI();
            StartCoroutine(GameManager.instance.FlashWeaponName(equippedWeapon));
        }
    }

    public void SpawnPlayer()
    {
        HP = originalHP;
        UpdateHPUI();

        characterController.enabled = false;
        if (GameManager.instance.playerSpawnPosition != null) transform.position = GameManager.instance.playerSpawnPosition.transform.position;
        characterController.enabled = true;
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        UpdateHPUI();

        if (HP <= 0) GameManager.instance.PlayerDead();
    }

    public void IncrementPoints(int value)
    {
        points += value;
        GameManager.instance.pointsText.text = "Points: " + points.ToString("000000000");
    }

    public void UpdateHPUI()
    {
        GameManager.instance.hpFill.fillAmount = (float)HP / originalHP;
    }

    void UpdateStaminaUI()
    {
        GameManager.instance.staminaFill.fillAmount = stamina / staminaMax;
    }

    void ToggleStaminaPie(bool value)
    {
        GameManager.instance.staminaFill.enabled = value;
    }

    public void ToggleCanSwitchWeapon(bool value)
    {
        canSwitchWeapon = value;
    }

    public void increasehealth(int amount)
    {
        HP += amount;
        UpdateHPUI();

        if (HP > originalHP)
        {
            HP = originalHP;
        }
    }





}
