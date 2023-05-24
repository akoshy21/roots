using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity; 

public enum PocketType
{
    Water,
    Nutrient
}

public class Pocket : MonoBehaviour
{
    public float drainRate;
    public float totalValue;
    private float currentValue;

    public PocketType type = PocketType.Water;

    [Header("Nutrient")] public Sprite grayedOut;
    
    [Header("Water")]
    public GameObject waterFill;
    
    private PlantManager pm;

    private Coroutine waterDrain;

    private Vector3 empty = new Vector3(0,-2f, 0);
    private float step = 0.1f;
    private float currStep = 1;

    private void Start()
    {
        pm = PlantManager.Instance;
        currentValue = totalValue;
    }

    public void DrainPocket()
    {
        if (currentValue > 0)
        {
            switch (type)
            {
                case PocketType.Water:
                    AudioManager.instance.PlayOneShot(FMOD_Events.instance.DrainPocket, this.transform.position);
                    if (waterDrain == null) waterDrain = StartCoroutine(DrainWaterOverTime());
                    break;
                case PocketType.Nutrient:

                    // TODO: play sound based on type of item.
                    currentValue -= drainRate;
                    pm.AddNutrient(drainRate);
                    break;
            }

   
        } else if (type == PocketType.Nutrient)
        {
            this.GetComponentInChildren<SpriteRenderer>().sprite = grayedOut;

        }
    }
    
    IEnumerator DrainWaterOverTime()
    {
        Debug.Log("Draining Water...");
        while (currentValue > 0)
        {
            currentValue -= drainRate;
            pm.AddWater(drainRate);
            waterFill.transform.localPosition = Vector3.Lerp(empty,Vector3.zero, currentValue/totalValue);
            yield return new WaitForSeconds(1);
        }

        if (currentValue <= 0) waterDrain = null;
    }

    public void SetTotal(float total)
    {
        totalValue = total;
    }
}