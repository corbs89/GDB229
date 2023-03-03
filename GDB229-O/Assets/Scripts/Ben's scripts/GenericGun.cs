using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGun : MonoBehaviour
{
    [SerializeField] int _damage = 100;
    [SerializeField] int _magSize = 30;
    [SerializeField] IGun.WeaponClass _weaponClass = IGun.WeaponClass.medium;
    [SerializeField] int _fireRate = 5;
    [SerializeField] int _range = 35;
    
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void Reload()
    {

    }    

    public int GetDamage() { return _damage; }
    public int GetWeight() { return (int)_weaponClass; }

    
}
