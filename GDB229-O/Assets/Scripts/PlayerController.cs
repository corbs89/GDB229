using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController characterController;

    [Header("-----Player Stats-----")]
    [SerializeField][Range(1f, 10f)] float playerSpeed;
    [SerializeField][Range(1, 5)] int jumpTimes;
    [SerializeField][Range(5, 50)] int jumpSpeed;
    [SerializeField][Range(10, 75)] int gravity;

    [Header("-----Gun Stats-----")]
    [SerializeField][Range(0.1f, 1f)] float shootRate;
    [SerializeField][Range(50, 1000)] int shootDistance;
    [SerializeField][Range(1, 100)] int shootDamage;

    int jumpsCurrent;
    Vector3 movement;
    Vector3 playerVelocity;
    bool isShooting;

    // we may need this later
    void Start()
    {
        
    }


    void Update()
    {
        ResetJump();
        ProcessMovement();
        ProcessJump();
        ProcessShoot();
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

    void ProcessShoot()
    {
        if (isShooting) { return; }

        if (Input.GetButton("Shoot"))
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().TakeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }
}
