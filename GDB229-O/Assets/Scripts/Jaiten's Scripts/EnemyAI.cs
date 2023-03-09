using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] bool isTarget;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] float attackSpeed;
    [SerializeField] int attackRange;
    [SerializeField] GameObject projectile;
    [SerializeField] int projectileSpeed;
    [SerializeField] Transform shootPos;

    Vector3 playerDirection;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetEnemyMovement();
        StartCoroutine(shoot());
    }

    void SetEnemyMovement()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            FacePlayer();
        }
    }

    void FacePlayer()
    {
        playerDirection = (GameManager.instance.player.transform.position - headPos.position);
        playerDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        StartCoroutine(FlashMat());

        if (HP <= 0 && isTarget == false)
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

    IEnumerator shoot()
    {
        isShooting = true;
        GameObject bulletClone = Instantiate(projectile, shootPos.position, projectile.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
        yield return new WaitForSeconds(attackSpeed);
        isShooting = false;
    }
}