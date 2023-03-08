using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIcons : MonoBehaviour
{
    [SerializeField] float rotateSpeed = .2f;

    // Update is called once per frame
    void Update()
    {
        rotateobject();
    }

    public void rotateobject()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.Self);
    }
}
