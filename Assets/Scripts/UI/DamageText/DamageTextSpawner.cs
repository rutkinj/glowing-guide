using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] Canvas damageText;
    [SerializeField] Vector3 offset;

    public void Spawn(float damageFloat){
        Canvas instance = Instantiate<Canvas>(damageText, transform);
        instance.GetComponentInChildren<TextMeshProUGUI>().SetText(damageFloat.ToString());
    }
}
