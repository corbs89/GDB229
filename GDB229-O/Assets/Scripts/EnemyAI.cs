using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] float attackSpeed;
    [SerializeField] int attackRange;
    [SerializeField] GameObject projectile;
    [SerializeField] int projectileSpeed;
    [SerializeField] Transform shootPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        StartCoroutine(FlashMat());

        if (HP <= 0)
        {
            StartCoroutine(FlashMat());
            StartCoroutine(DestroyObject());
            
        }
    }

    IEnumerator FlashMat()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;

    }
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
