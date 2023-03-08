using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyMovement : MonoBehaviour
{

    [Range(1, 10)][SerializeField] int movementSpeed;
    Vector3 position;

    private void Start() {
       position = transform.position;
    }
    
    private void Update()
    {

        transform.Translate(-1 * movementSpeed * Time.deltaTime, 0, 0, Space.Self);

        if (transform.position.x < -5)
        {

            transform.Translate(1 * movementSpeed * Time.deltaTime, 0, 0, Space.Self);

        }
    }
}

