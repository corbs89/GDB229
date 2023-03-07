using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController characterController;

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
    [SerializeField] Gun weaponSlot1;
    [SerializeField] Gun weaponSlot2;
    [SerializeField] Gun equippedWeapon;
    [SerializeField][Range(0f, 10f)] float weightModifier;


    int originalHP;
    float staminaMax;
    float timeSinceUsedStamina = Mathf.Infinity;
    int jumpsCurrent;
    Vector3 movement;
    Vector3 playerVelocity;
    int points;
    
    public int GetHP() { return HP; }
    public float GetStamina() { return stamina; }
    public int GetPoints() { return points; }

    void Start()
    {
        originalHP = HP;
        staminaMax = stamina;
        UpdateHPUI();
        SpawnPlayer();
    }


    void Update()
    {
        ResetJump();
        ProcessMovement();
        ProcessJump();
        IncrementStamina();

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
        }

        stamina = Mathf.Clamp(stamina, 0f, staminaMax);
        UpdateStaminaUI();
    }

    void DecrementStamina()
    {
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

    void EquipWeapon(Gun weapon)
    {
        if (weapon != null)
        {
            equippedWeapon = weapon;
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

    void UpdateHPUI()
    {
        GameManager.instance.hpSlider.value = HP;
    }

    void UpdateStaminaUI()
    {
        GameManager.instance.staminaSlider.value = stamina;
    }

}
