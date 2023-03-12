using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyMovement : MonoBehaviour
{

    public float speed;
    bool isMovingLeft;
    bool isMovingRight;

    void Start()
    {
        isMovingLeft = true;
        isMovingRight = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            dummyMove();
        }
    }
    private void dummyMove()
    {
        if (isMovingLeft == true)
        {
            Moveleft();
            if (transform.position.x <= -5)
            {
                isMovingLeft = false;
                isMovingRight = true;
            }
        }
        else
        {
            Moveright();
            if (transform.position.x >= 5)
            {
                isMovingRight = false;
                isMovingLeft = true;
                Moveleft();
            }
        }
    }
    private void Moveleft()
    {
        transform.Translate(-1 * speed * Time.deltaTime, 0, 0, Space.Self);
    }
    private void Moveright()
    {
        transform.Translate(1 * speed * Time.deltaTime, 0, 0, Space.Self);
    }

}