using System.Collections;
using System.Collections.Generic;
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

   void Update()
   {
        if (!isShooting && Input.GetButton("Shoot"))
        {
            Debug.Log("isShooting!");
            StartCoroutine(Shoot());
        }

    }


    IEnumerator Shoot()
    {

        isShooting = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, data.range))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                Debug.Log("hit");
                hit.collider.GetComponent<IDamage>().TakeDamage(data.damage);
            }
        }

        yield return new WaitForSeconds(data.fireRate);
        isShooting = false;

    }

    public int GetDamage() { return data.damage; }
    public int GetWeight() { return (int) data.weaponClass; }
}
