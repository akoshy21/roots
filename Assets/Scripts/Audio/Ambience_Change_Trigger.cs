using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience_Change_Trigger : MonoBehaviour
{
    [Header("Paramter Change")]

    [SerializeField] private string ParameterName;

    [SerializeField] private float ParameterValue; 

    private void OnTriggerEnter2D(Collider2D colllider)
    {
        if (colllider.tag.Equals("Player"))
        {
            AudioManager.instance.SetAmbienceParameter(ParameterName, ParameterValue);
        }
    }
}
