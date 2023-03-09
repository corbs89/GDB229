using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LootDrops : MonoBehaviour
{
    [SerializeField][Range(0, 100)] int dropChance;
    [SerializeField] List<GameObject> lootDrops;

    public void LootDrop()
    {
        int randomChance = Random.Range(0, 101);

        if (randomChance <= dropChance)
        {
            int index = Random.Range(0, lootDrops.Count);
            Debug.Log(randomChance);
            GameObject item = Instantiate(lootDrops[index]);
            item.transform.position = transform.position + new Vector3(0f, 1f, 0f);
        }
        
    }
}
