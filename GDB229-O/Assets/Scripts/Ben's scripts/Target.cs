using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] Material material;
    [SerializeField] TargetSpawn manager;

    [Header("----- Target Stats -----")]
    [SerializeField] int HP;
    int currentHP;


    // Start is called before the first frame update
    void Start()
    {
        currentHP = HP;
    }

    // Update is called once per frame


    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("current hp: " + currentHP);
        StartCoroutine(FlashMat());

        if (currentHP <= 0)
        {
            StartCoroutine(FlashMat());
            StartCoroutine(DestroyObject());
            //Invoke("DestroyObject", 0.3f);
        }
    }

    IEnumerator FlashMat()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material = material;

    }
    IEnumerator DestroyObject()
    {
        StartCoroutine(FlashMat());
        yield return new WaitForSeconds(0.3f);
        manager.DestroyTarget();
        currentHP = HP;

    }
}
