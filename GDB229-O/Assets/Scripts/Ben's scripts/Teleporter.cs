using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("-----Collision-----")]
    [SerializeField] Renderer model;
    [Header("-----Level-----")]
    [SerializeField] [Range(0, 5)] int levelSelect;
    //[SerializeField] TeleportData data;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ButtonFunctions.LoadLevel(levelSelect);
            StartCoroutine(FlashMat());
        }
    }
    IEnumerator FlashMat()
    {
        Debug.Log("Flash");
        model.material.color = Color.green;
        yield return new WaitForSeconds(0.3f);
        model.material.color = Color.white;

    }
}
