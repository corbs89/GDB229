using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController characterController;

    [Header("-----Player Stats-----")]
    [SerializeField][Range(0f, 10f)] int HP;
    [SerializeField][Range(0f, 10f)] int stamina;
    [SerializeField][Range(1f, 10f)] float playerSpeed;
    [SerializeField][Range(1f, 2f)] float sprintCoefficient;
    [SerializeField][Range(1, 5)] int jumpTimes;
    [SerializeField][Range(5, 50)] int jumpSpeed;
    [SerializeField][Range(10, 75)] int gravity;

    [Header("-----Gun Stats-----")]
    [SerializeField] GenericGun weaponSlot1;
    [SerializeField] GenericGun weaponSlot2;
    [SerializeField] GenericGun equippedWeapon;
    [SerializeField][Range(0f, 10f)] float weightModifier;


    int originalHP;
    int jumpsCurrent;
    Vector3 movement;
    Vector3 playerVelocity;
    int points;
    
    public int GetHP() { return HP; }
    public int GetStamina() { return stamina; }
    public int GetPoints() { return points; }

    void Start()
    {
        originalHP = HP;
        SpawnPlayer();
    }


    void Update()
    {
        ResetJump();
        ProcessMovement();
        ProcessJump();
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

        characterController.Move(currentSpeed * Time.deltaTime * movement);
        Debug.Log(currentSpeed);
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

    public void SpawnPlayer()
    {
        HP = originalHP;
        UpdateUI();

        characterController.enabled = false;
        if (GameManager.instance.playerSpawnPosition != null) transform.position = GameManager.instance.playerSpawnPosition.transform.position;
        characterController.enabled = true;
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        UpdateUI();

        if (HP <= 0) GameManager.instance.PlayerDead();

    }

    public void IncrementPoints(int value)
    {
        points += value;
        UpdateUI();
    }

    void UpdateUI()
    {
        GameManager.instance.hpSlider.value = HP;
        GameManager.instance.staminaSlider.value = stamina;
        GameManager.instance.SetPoints(points);
    }
}
