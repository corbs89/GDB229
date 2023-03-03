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
    [SerializeField][Range(1, 5)] int jumpTimes;
    [SerializeField][Range(5, 50)] int jumpSpeed;
    [SerializeField][Range(10, 75)] int gravity;

    [Header("-----Gun Stats-----")]
    [SerializeField][Range(0.1f, 1f)] float shootRate;
    [SerializeField][Range(50, 1000)] int shootDistance;
    [SerializeField][Range(1, 100)] int shootDamage;


    int originalHP;
    int jumpsCurrent;
    Vector3 movement;
    Vector3 playerVelocity;
    bool isShooting;
    
    public int GetHP() { return HP; }
    public int GetStamina() { return stamina; }

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

        characterController.Move(playerSpeed * Time.deltaTime * movement);
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

    void UpdateUI()
    {
        GameManager.instance.hpSlider.value = HP;
    }
}
