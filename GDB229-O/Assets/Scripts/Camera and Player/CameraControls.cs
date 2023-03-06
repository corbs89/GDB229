using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Header("-----Sensitivity-----")]
    [SerializeField] int sensitivityH;
    [SerializeField] int sensitivityV;

    [Header("-----Vertical Min / Max-----")]
    [SerializeField] int verticalMin;
    [SerializeField] int verticalMax;

    [Header("-----Invert-----")]
    [SerializeField] bool invertY;

    [Header("-----Camera Shake-----")]
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeMagnitude;

    float xRotation;
    Vector3 initialPosition;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        initialPosition = transform.position;
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
    public void CameraShake()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float timeElapsed = 0f;

        while (timeElapsed < shakeDuration)
        {
            transform.position = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            timeElapsed += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        transform.position = initialPosition;
    }
}
