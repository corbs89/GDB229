using System.Collections;
using System.Collections.Generic;
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
    bool isShooting;
    int CurrentMagCount;
    int CurrentReserveCount;
    int MaxMagCount;
    int MaxReserveCount;
    float TimeBetweenShots;

    private void Start()
    {
        MaxReserveCount = data.reserveSize;
        MaxMagCount= data.magSize;
        CurrentReserveCount = MaxReserveCount;
        CurrentMagCount = MaxMagCount;

        float rps = data.fireRate / 60;
        TimeBetweenShots = 1 / rps;
        UpdateUI();

    }
    void Update()
   {
        if (!isShooting && Input.GetButton("Shoot") && CurrentMagCount > 0)
        {
            Debug.Log("isShooting!");
            StartCoroutine(Shoot());
        }
        else if(CurrentMagCount <= 0 && Input.GetButton("Shoot"))
        {
            Reload();
        }
        if(Input.GetButtonDown("Reload"))
        {
            Reload();
        }

    }
    void Reload()
    {
        int bulletsShot = data.magSize - CurrentMagCount;
        Debug.Log("Bullets shot: " + bulletsShot);
        if(CurrentReserveCount > 0)
        {
            if(CurrentReserveCount < data.magSize)
            {
                CurrentMagCount += bulletsShot;
                CurrentReserveCount -= bulletsShot;
                Debug.Log(CurrentMagCount);
            }
            else
            {
                CurrentMagCount = data.magSize;
                CurrentReserveCount -= bulletsShot;
            }

        }
        else
        {
            Debug.Log("Reserves Empty");
        }
        Debug.Log("Reserve Count: " + CurrentReserveCount);
        GameManager.instance.UpdateReserve(CurrentReserveCount);
        GameManager.instance.UpdateMagazine(CurrentMagCount);
    }

    IEnumerator Shoot()
    {

        isShooting = true;
        CameraControls.instance.CameraShake(data.damage, 0.0675f);
        CurrentMagCount--;
        GameManager.instance.UpdateMagazine(CurrentMagCount);
        RaycastHit hit;
        Debug.Log("currentMag: " + CurrentMagCount);
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, data.range))
        {
            

            if (hit.collider.GetComponent<IDamage>() != null)
            {
                Debug.Log("hit");
                hit.collider.GetComponent<IDamage>().TakeDamage(data.damage);
            }
        }

        yield return new WaitForSeconds(TimeBetweenShots);
        isShooting = false;

    }
    void UpdateUI()
    {
        GameManager.instance.UpdateMagazine(CurrentMagCount);
        GameManager.instance.UpdateReserve(CurrentReserveCount);
    }

    public int GetDamage() { return data.damage; }
    public int GetWeight() { return (int) data.weaponClass; }
}
