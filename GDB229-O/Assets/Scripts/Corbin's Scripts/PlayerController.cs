using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    [SerializeField][Range(0f, 10f)] float weightModifier;


    int originalHP;
    float staminaMax;
    float timeSinceUsedStamina = Mathf.Infinity;
    int jumpsCurrent;
    Vector3 movement;
    Vector3 playerVelocity;
    int points;
    public bool canSwitchWeapon = true;
    movementState state;

    enum movementState
    {
        normal,
        sprinting,
        exhausted,
        jumping,
        jumpsprint
    }
    
    public Gun equippedWeapon;

    public int GetHP() { return HP; }
    public float GetStamina() { return stamina; }
    public int GetPoints() { return points; }
    public GameObject GetGunPosition() { return gunPosition; }

    public Gun GetEquippedWeapon() {  return equippedWeapon; }

    void Start()
    {
        originalHP = HP;
        staminaMax = stamina;
        UpdateHPUI();
        SpawnPlayer();
        state = movementState.normal;
    }


    void Update()
    {
        ResetJump();
        ProcessSprint();
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
            state = movementState.normal;
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

        if (state == movementState.sprinting && stamina != 0 && movement != Vector3.zero)
        {
            currentSpeed *= sprintCoefficient;
            DecrementStamina();
        }
        else if (state == movementState.jumpsprint)
        {
            currentSpeed *= sprintCoefficient;
        }

        characterController.Move(currentSpeed * Time.deltaTime * movement);
    }

    void ProcessSprint()
    {
        if (state == movementState.exhausted || state == movementState.jumping || state == movementState.jumpsprint) return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            state = movementState.sprinting;
        }
        else
        {
            state = movementState.normal;
        }
    }

    void IncrementStamina()
    {
        if (timeSinceUsedStamina > staminaRechargeStartTime)
        {
            stamina += staminaRechargeRate * Time.deltaTime;

            stamina = Mathf.Clamp(stamina, 0f, staminaMax);

            if (stamina == staminaMax)
            {
                ToggleStaminaPie(false);
                state = movementState.normal;
                GameManager.instance.staminaFill.color = new Color(255, 255, 255, 150);
            }
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

        if (stamina <= Mathf.Epsilon)
        {
            state = movementState.exhausted;
            GameManager.instance.staminaFill.color = new Color(255, 0, 0, 150);
        }
    }

    void ProcessJump()
    {
        if (Input.GetButtonDown("Jump") && jumpsCurrent < jumpTimes)
        {
            playerVelocity.y = jumpSpeed;
            jumpsCurrent++;
            if (state == movementState.sprinting) state = movementState.jumpsprint;
            else state = movementState.jumping;
        }

        playerVelocity.y -= gravity * Time.deltaTime;

        characterController.Move(Time.deltaTime * playerVelocity);
    }

    void SwapWeapons()
    {
        if (equippedWeapon == null) return;

        if (Input.GetKeyDown(KeyCode.Q) && canSwitchWeapon)
        {
            equippedWeapon.enabled = false;

            if (equippedWeapon == LoadoutManager.instance.weaponSlot1)
            {
                LoadoutManager.instance.slot1Renderer.enabled = false;
                LoadoutManager.instance.slot2Renderer.enabled = true;
                equippedWeapon = LoadoutManager.instance.weaponSlot2;
            }
            else if (equippedWeapon == LoadoutManager.instance.weaponSlot2)
            {
                LoadoutManager.instance.slot1Renderer.enabled = true;
                LoadoutManager.instance.slot2Renderer.enabled = false;
                equippedWeapon = LoadoutManager.instance.weaponSlot1;
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
        StartCoroutine(GameManager.instance.playerHit());

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

        if (HP > originalHP)
        {
            HP = originalHP;
        }

        UpdateHPUI();
    }





}
