using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController characterController;

    [Header("-----Objects-----")]
    /*[SerializeField] GameObject gunPosition;*/

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
    [SerializeField] List<weaponStats> weaponsList = new List<weaponStats>();
    [SerializeField][Range(0f, 10f)] float weightModifier;
    [SerializeField] float shootRate;
    [SerializeField] float shootDistance;
    [SerializeField] float weapondamage;
    [SerializeField] MeshFilter weaponModel;
    [SerializeField] MeshRenderer weaponMaterial;
    [SerializeField] AudioClip weaponSFX;

    [Header("-----Camera Stats-----")]
    public float maxZoom;
    public float zoomInSpeed;
    public float zoomOutSpeed;


    /*
        [SerializeField] GameObject PistolPickupPrefab;
        [SerializeField] GameObject ARPickupPrefab;
        [SerializeField] GameObject SniperPickupPrefab;*/

    int originalHP;
    float staminaMax;
    float timeSinceUsedStamina = Mathf.Infinity;
    int jumpsCurrent;
    Vector3 movement;
    Vector3 playerVelocity;
    int points;
    public bool canSwitchWeapon = true;
    movementState state;
    bool screenIsFlashing;
    float originalCameraZoom;
    public int selectedWeapon;

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
    //public GameObject GetGunPosition() { return gunPosition; }

    public Gun GetEquippedWeapon() { return equippedWeapon; }

    void Awake()
    {
        originalHP = HP;
        staminaMax = stamina;
        //UpdateHPUI();
        //SpawnPlayer();
        state = movementState.normal;
    }


    void Update()
    {
        ResetJump();
        ProcessSprint();
        ProcessMovement();
        ProcessJump();
        IncrementStamina();
        WeaponSelect();
        // SwapWeapons();
        StartCoroutine(CheckForLowHealth());
        //ProcessWeaponDrop();

        timeSinceUsedStamina += Time.deltaTime;
    }

    #region Movement
   
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

    #region Jumping
    void ResetJump()
    {
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            jumpsCurrent = 0;
            playerVelocity.y = 0;
            state = movementState.normal;
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
    #endregion

    #region Sprinting
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

    #endregion

    #endregion

    #region UI
    public void IncrementPoints(int value)
    {
        points += value;
        GameManager.instance.pointsText.text = points.ToString("000000");
    }

    public void UpdateHPUI()
    {
        Debug.Log(HP);
        Debug.Log(GameManager.instance.hpFill);
        Debug.Log(GameManager.instance.playerController);
        Debug.Log(originalHP);
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

    IEnumerator CheckForLowHealth()
    {
        float percentHP = (float)HP / originalHP;

        if (percentHP < .25f && !screenIsFlashing)
        {
            screenIsFlashing = true;

            while (percentHP <= .25f)
            {
                StartCoroutine(GameManager.instance.playerHit(0.3f, 0f));

                yield return new WaitForSeconds(5);

                percentHP = (float)HP / originalHP;
            }

            screenIsFlashing = false;
        }
    }
    #endregion

    #region Health
    public void TakeDamage(int damage)
    {
        HP -= damage;
        UpdateHPUI();
        StartCoroutine(GameManager.instance.playerHit(0.1f, 0.0675f));

        if (HP <= 0) GameManager.instance.PlayerDead();
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
    #endregion

    #region Spawning
    public void SpawnPlayer()
    {
        Reset();

        characterController.enabled = false;
        if (GameManager.instance.playerSpawnPosition != null) transform.position = GameManager.instance.playerSpawnPosition.transform.position;
        characterController.enabled = true;
    }
    public void Reset()
    {
        HP = originalHP;
        points = 0;
        stamina = staminaMax;

        UpdateHPUI();
        UpdateStaminaUI();
    }
    #endregion

    #region New Weapon Stuff
    //Adds the picked up weapon to a list and retieves and displays the weapon data. Keeps track of the weapons in the list
    public void WeaponPickup(weaponStats weapon)
    {
        weaponsList.Add(weapon);

        shootRate = weapon.shootRate;
        shootDistance = weapon.shootDistance;
        weightModifier = weapon.weightModifier;
        weapondamage = weapon.weapondamage;

        weaponModel.sharedMesh = weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponMaterial.sharedMaterial = weaponModel.GetComponent<MeshRenderer>().sharedMaterial;

        selectedWeapon = weaponsList.Count - 1;

    }
    //Allows for switching from one weapon is the list to the other from Input.
    void WeaponSelect()
    {
        if (GameManager.instance.isPaused == false)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < weaponsList.Count - 1)
            {
                selectedWeapon++;
                ChangeWeapon();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > 0)
            {
                selectedWeapon--;
                ChangeWeapon();
            }
        }
    }
    //Updates the weapons stats for each weapon when the player switches weapon 
    void ChangeWeapon()
    {
        shootRate = weaponsList[selectedWeapon].shootRate;
        shootDistance = weaponsList[selectedWeapon].shootDistance;
        weapondamage = weaponsList[selectedWeapon].weapondamage;
        weightModifier = weaponsList[selectedWeapon].weightModifier;


        weaponModel.sharedMesh = weaponsList[selectedWeapon].weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponMaterial.sharedMaterial = weaponsList[selectedWeapon].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;

    }
    #endregion

    #region Old Weapon Stuff (for reference)
    /*   void ProcessWeaponDrop()
       {
           if (equippedWeapon != null && canSwitchWeapon && Input.GetKeyDown(KeyCode.G))
           {

               switch (equippedWeapon.GetWeight())
               {
                   case 0:
                       SpawnWeaponPickup(PistolPickupPrefab);
                       break;

                   case 25:
                       break;

                   case 50:
                       SpawnWeaponPickup(ARPickupPrefab);
                       break;

                   case 75:
                       SpawnWeaponPickup(SniperPickupPrefab);
                       break;
               }

               if (equippedWeapon == LoadoutManager.instance.weaponSlot1)
               {

                   LoadoutManager.instance.ClearSlot1();
                   if (LoadoutManager.instance.weaponSlot2 != null)
                   {
                       LoadoutManager.instance.slot2Renderer.enabled = true;
                       equippedWeapon = LoadoutManager.instance.weaponSlot2;
                       equippedWeapon.enabled = true;
                       equippedWeapon.UpdateUI();
                       LoadoutManager.instance.slot = LoadoutManager.Slot.two;
                   }
                   else
                   {
                       ClearEquippedWeapon();
                   }
               }
               else if (equippedWeapon == LoadoutManager.instance.weaponSlot2)
               {
                   LoadoutManager.instance.ClearSlot2();
                   if (LoadoutManager.instance.weaponSlot1 != null)
                   {
                       LoadoutManager.instance.slot1Renderer.enabled = true;
                       equippedWeapon = LoadoutManager.instance.weaponSlot1;
                       equippedWeapon.enabled = true;
                       equippedWeapon.UpdateUI();
                       LoadoutManager.instance.slot = LoadoutManager.Slot.one;
                   }
                   else
                   {
                       ClearEquippedWeapon();
                   }
               }
           }
       }*/

    void SpawnWeaponPickup(GameObject weapon)
    {
        GameObject item;

        item = Instantiate(weapon);
        item.transform.position = transform.position;
        item.GetComponent<SphereCollider>().enabled = false;
        StartCoroutine(item.GetComponent<WeaponPickup>().EnablePickup());
    }

    public void ClearEquippedWeapon()
    {
        equippedWeapon = null;
        GameManager.instance.UpdateMagazine(0);
        GameManager.instance.UpdateReserve(0);
        LoadoutManager.instance.slot = LoadoutManager.Slot.none;
    }

    /* void SwapWeapons()
    {
        if (equippedWeapon == null || LoadoutManager.instance.weaponSlot1Object == null || LoadoutManager.instance.weaponSlot2Object == null) return;

        if (Input.GetKeyDown(KeyCode.Q) && canSwitchWeapon)
        {
            equippedWeapon.enabled = false;

            if (equippedWeapon == LoadoutManager.instance.weaponSlot1)
            {
                LoadoutManager.instance.slot1Renderer.enabled = false;
                LoadoutManager.instance.slot2Renderer.enabled = true;
                equippedWeapon = LoadoutManager.instance.weaponSlot2;
                LoadoutManager.instance.slot = LoadoutManager.Slot.two;
            }
            else if (equippedWeapon == LoadoutManager.instance.weaponSlot2)
            {
                LoadoutManager.instance.slot1Renderer.enabled = true;
                LoadoutManager.instance.slot2Renderer.enabled = false;
                equippedWeapon = LoadoutManager.instance.weaponSlot1;
                LoadoutManager.instance.slot = LoadoutManager.Slot.one;
            }

            equippedWeapon.enabled = true;
            equippedWeapon.UpdateUI();
            StartCoroutine(GameManager.instance.FlashWeaponName(equippedWeapon));
        }
    }*/
    public void ToggleCanSwitchWeapon(bool value)
    {
        canSwitchWeapon = value;
    }
    #endregion





















}
