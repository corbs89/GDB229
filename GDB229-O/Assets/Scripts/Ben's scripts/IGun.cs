using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    enum WeaponClass
    {
        Pistol = 0,
        light = 25,
        medium = 50,
        Heavy = 75
    }

    void Shoot();

    void Reload();

    int GetDamage();
    int GetWeight();
}
