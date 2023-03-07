using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetSpawn : MonoBehaviour
{
    [SerializeField] GameObject ObjectToSpawn;
    [SerializeField] int SpawnDelay;

    public GameObject activeTarget;

    //public GameObject activeTarget;
    //public TargetSpawn spawner;
    
    // Start is called before the first frame update

   
    void Start()
    {
        activeTarget = ObjectToSpawn;
        activeTarget.SetActive(true);
        
    }

    // Update is called once per frame
    
    bool CheckActive()
    {
        bool isActive = false;
        if (activeTarget != null) 
        {
            isActive = true;
        }
        return isActive;
    }
    void Spawn()
    {
        if(!CheckActive())
        {
            Invoke("ObjSpawn", 3.0f);
        }
    }
    
    public void DestroyTarget()
    {
        if (activeTarget != null) 
        {
            Debug.Log("Object dead");
            activeTarget.SetActive(false);
            activeTarget = null;
            Spawn();
        }
        
    }
    
    void ObjSpawn()
    { 
        Debug.Log("Object alive again");
        activeTarget = ObjectToSpawn;
        activeTarget.SetActive(true);
        
    }
    
}
