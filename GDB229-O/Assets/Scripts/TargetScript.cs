using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [Header("-----Components-----")]
    [SerializeField] Renderer model;

    public void takeDamage(int dmg)
    {
        StartCoroutine(flashMat());
    }

    IEnumerator flashMat()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
