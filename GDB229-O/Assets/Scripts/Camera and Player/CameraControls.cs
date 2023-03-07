using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public static CameraControls instance;

    [Header("-----Sensitivity-----")]
    [SerializeField] int sensitivityH;
    [SerializeField] int sensitivityV;

    [Header("-----Vertical Min / Max-----")]
    [SerializeField] int verticalMin;
    [SerializeField] int verticalMax;

    [Header("-----Invert-----")]
    [SerializeField] bool invertY;

    float shakeDuration = 0.01f;
    float shakeMagnitudeMultiplier = 0.01f;

    float xRotation;
    Vector3 initialPosition;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        initialPosition = transform.localPosition;
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
    public void CameraShake(int damage, float duration)
    {
        StartCoroutine(Shake(damage, duration));
    }

    IEnumerator Shake(int magnitude, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.localPosition = initialPosition + new Vector3(Random.Range(-.25f, .25f), Random.Range(-.25f, .25f), 0) * (float)magnitude * shakeMagnitudeMultiplier;

            timeElapsed += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = initialPosition;
    }
}
