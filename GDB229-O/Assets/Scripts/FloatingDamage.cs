using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamage : MonoBehaviour
{
    TextMeshPro damageAmountText;
    [SerializeField] Transform textSpawnPosition;

    IEnumerator TextDestroy()
    {
        yield return new WaitForSeconds(0.8f);
    }

    public void SetDamageText(int damageAmount)
    {
        damageAmountText.text = damageAmount.ToString();
    }
}
