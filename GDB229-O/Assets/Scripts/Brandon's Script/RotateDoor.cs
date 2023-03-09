using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class RotateDoor : MonoBehaviour, Iinteract
{
    
    public bool isOpen;
    public bool isLeftDoor;
    public int speed = 4;
    Vector3 forward;
    Vector3 UserPosition;

    Coroutine AnimationCoroutine;

    void Awake()
    {
       

    }
    
    public void Interact()
    {
        UserPosition = GameManager.instance.playerController.transform.position;
        
        if (!isOpen) 
        {
            
            if (AnimationCoroutine != null)
            {
                
                StopCoroutine(AnimationCoroutine);

                
            }

           float dot = Vector3.Dot(forward, (UserPosition - transform.position).normalized);
           AnimationCoroutine = StartCoroutine(DoRotationOpen());
             

            
        }
       
    }
    IEnumerator DoRotationOpen()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(new Vector3(0, -118, 0));
        float time = 0;
        while(time <1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
    





}
