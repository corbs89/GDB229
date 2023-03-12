using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class weaponStats : ScriptableObject
{
    public GameObject weaponModel;
    public int shootRate;
    public int shootDistance;
    public int weapondamage;
    public int weightModifier;
    public int purchasePrice;
    public int maxMagSize;
    public AudioClip gunShotSFX;

}
