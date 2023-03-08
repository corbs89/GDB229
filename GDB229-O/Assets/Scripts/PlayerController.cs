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
    [SerializeField] GameObject weaponSlot1;
    [SerializeField] GameObject weaponSlot2;
    [SerializeField][Range(0f, 10f)] float weightModifier;


    int originalHP;
    float staminaMax;
    float timeSinceUsedStamina = Mathf.Infinity;
    int jumpsCurrent;
    Vector3 movement;
    Vector3 playerVelocity;
    int points;

    Gun slot1;
    Gun slot2;
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

        gunPosition.transform.Rotate(new Vector3(0, -90, 0));

        slot1 = Instantiate(weaponSlot1.transform.GetComponentInChildren<Gun>(), gunPosition.transform);
        slot2 = Instantiate(weaponSlot2.transform.GetComponentInChildren<Gun>(), gunPosition.transform);

        slot1Renderer = slot1.GetComponent<Renderer>();
        slot2Renderer = slot2.GetComponent<Renderer>();

        slot1.transform.localScale = Vector3.one;
        slot1.transform.localPosition = Vector3.zero;
        slot2.transform.localScale = Vector3.one;
        slot2.transform.localPosition = Vector3.zero;

        slot1.enabled = false;
        slot2.enabled = false;
        slot2Renderer.enabled = false;

        equippedWeapon = slot1;
        equippedWeapon.enabled = true;
    }


    void Update()
    {
        ResetJump();
        ProcessMovement();
        ProcessJump();
        IncrementStamina();
        EquipLastWeapon();

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

    void EquipWeapon(Gun weapon)
    {
        if (weapon != null)
        {
            equippedWeapon = weapon;
            StartCoroutine(GameManager.instance.FlashWeaponName(equippedWeapon));
        }
    }


    void EquipLastWeapon ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            equippedWeapon.enabled = false;

            if (equippedWeapon == slot1)
            {
                slot1Renderer.enabled = false;
                slot2Renderer.enabled = true;
                equippedWeapon = slot2;
            }
            else if (equippedWeapon == slot2)
            {
                slot1Renderer.enabled = true;
                slot2Renderer.enabled = false;
                equippedWeapon = slot1;
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

    void UpdateHPUI()
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
}
