using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("----- Bullet Stats -----")]
    [SerializeField] int damage;
    [SerializeField] int timer;

    private void Start()
    {
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerController.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
