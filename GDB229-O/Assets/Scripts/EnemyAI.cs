using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        setEnemyMovement();
    }

    void setEnemyMovement()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            facePlayer();
        }
    }

    void facePlayer()
    {
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
}
