using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] int sensitivityH;
    [SerializeField] int sensitivityV;

    [SerializeField] int verticalMin;
    [SerializeField] int verticalMax;

    [SerializeField] bool invertY;

    float xRotation;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivityH;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityV;

        if (invertY) xRotation += mouseY; 
        else xRotation -= mouseY; 

        xRotation = Mathf.Clamp(xRotation, verticalMin, verticalMax);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
       
    }
}
