using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [Header("-----Put all spawner in the scene here-----")]
    [SerializeField] List<GameObject> spawners;
    [Header("-----Put all enemy types to spawn here-----")]
    [SerializeField] List<GameObject> spawnObjects;
    [Header("-----Spawn Settings-----")]
    [SerializeField] float spawnDelayMin;
    [SerializeField] float spawnDelayMax;
    [SerializeField] float spawnRateChangeOnRound;
    

    int round;
    int totalRounds;
    int randomSpawner;
    float randomDelay;
    GameObject randomEnemy;


    private void Awake()
    {
        instance = this; ;
    }
    // Start is called before the first frame update
    void Start()
    {
        totalRounds = GameManager.instance.currentRound;

    }

    public void TriggerSpawn()
    {
        

        round = GameManager.instance.currentRound;
        Debug.Log("Current round: " + round);

        int roundsPast = totalRounds - round;
        if(spawners.Count > 0 && spawnObjects.Count > 0 && GameManager.instance.CanSpawnEnemy()) 
        {
           
            
                
                randomDelay = Random.Range(Mathf.Clamp(spawnDelayMin - (spawnRateChangeOnRound * roundsPast), 0, Mathf.Infinity) , Mathf.Clamp(spawnDelayMax - (spawnRateChangeOnRound * roundsPast), 0, Mathf.Infinity));
                
                randomEnemy = spawnObjects[Random.Range(0, spawnObjects.Count)];

                randomSpawner = Random.Range(0, spawners.Count);
                
                Invoke("CallSpawn", randomDelay);

            


        }

    }
    void CallSpawn()
    {
        //spawners[randomSpawner].Spawn(randomEnemy);
       
       Instantiate(randomEnemy, spawners[randomSpawner].transform.position, spawners[randomSpawner].transform.rotation);
       Debug.Log("Object spawned");

        TriggerSpawn();    

    }
   
}
