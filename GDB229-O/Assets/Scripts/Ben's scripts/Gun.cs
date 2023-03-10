using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public enum WeaponClass
{
    Pistol = 0,
    light = 25,
    medium = 50,
    Heavy = 75
}

public class Gun : MonoBehaviour
{

    [SerializeField] GunData data;
    [SerializeField] Transform weaponSnap;
    bool isShooting;
    bool canShoot;
    float reloadSpeed;
    int CurrentMagCount;
    int CurrentReserveCount;
    int MaxMagCount;
    int MaxReserveCount;
    float TimeBetweenShots;

    private void Start()
    {
        MaxReserveCount = data.reserveSize;
        MaxMagCount = data.magSize;
        CurrentReserveCount = MaxReserveCount;
        CurrentMagCount = MaxMagCount;
        reloadSpeed = data.reloadSpeed;
        canShoot = true;
        float rps = data.fireRate / 60;
        TimeBetweenShots = 1 / rps;
        UpdateUI();

    }
    void Update()
    {
        if (canShoot)
        {
            if (!isShooting && Input.GetButton("Shoot") && CurrentMagCount > 0)
            {
                
                StartCoroutine(Shoot());
            }
            else if (CurrentMagCount <= 0 && Input.GetButton("Shoot"))
            {
                StartCoroutine(Reload());
            }
            if (Input.GetButtonDown("Reload") && !(CurrentMagCount == MaxMagCount))
            {
                StartCoroutine(Reload());
            }
            if(Input.GetButtonDown("Interact"))
            {
                CallInteract();
            }
        }

    }
    void CallInteract()
    {
        Debug.Log("output Called");
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, data.range))
        {
            Debug.Log("Raycast hit");
            hit.collider.GetComponent<Iinteract>()?.Interact();
        }
    }
    IEnumerator Reload()
    {
        canShoot = false;
        
        StartCoroutine(GameManager.instance.StartReloadMeter());

        yield return new WaitForSecondsRealtime(reloadSpeed);

        int bulletsShot = data.magSize - CurrentMagCount;
        
        if (CurrentReserveCount <= 0)
        {
            
            StartCoroutine(GameManager.instance.FlashReservewarning());
        }
        else if (CurrentReserveCount < bulletsShot)
        {
            CurrentMagCount += CurrentReserveCount;
            CurrentReserveCount = 0;
            
        }
        else
        {
            CurrentMagCount += bulletsShot;
            CurrentReserveCount -= bulletsShot;
        }

        
        UpdateUI();
        canShoot = true;
    }

    IEnumerator Shoot()
    {

        isShooting = true;
        CameraControls.instance.CameraShake(data.damage, 0.0675f);
        CurrentMagCount--;
        GameManager.instance.UpdateMagazine(CurrentMagCount);
        RaycastHit hit;
        
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, data.range))
        {


            if (hit.collider.GetComponent<IDamage>() != null)
            {
                
                hit.collider.GetComponent<IDamage>().TakeDamage(data.damage);
            }
        }

        yield return new WaitForSeconds(TimeBetweenShots);
        isShooting = false;

    }
    public void UpdateUI()
    {
        GameManager.instance.UpdateMagazine(CurrentMagCount);
        GameManager.instance.UpdateReserve(CurrentReserveCount);
    }
    public void AddAmmo(int _ammoToAdd)
    {
        if (_ammoToAdd + CurrentReserveCount >= MaxReserveCount)
        {
            int canTake = MaxReserveCount - CurrentReserveCount;
            if (canTake > 0)
            {
                CurrentReserveCount += canTake;
            }
        }
        else
        {
            CurrentReserveCount += _ammoToAdd;
        }
        UpdateUI();
    }

    public void refillAmmo()
    {
        CurrentMagCount = MaxMagCount;
        CurrentReserveCount = MaxReserveCount;
        UpdateUI();
    }



    public int GetDamage() { return data.damage; }
    public int GetWeight() { return (int)data.weaponClass; }
    public float GetReloadSpeed() { return data.reloadSpeed; }
}
