using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public float waterCurrent, waterMax;
    public float nutrientCurrent, nutrientMax;

    public float decayRateWater, decayRateNutrients;

    public CustomFillBar nutrientsBar, waterBar;

    private List<DecayListener> _listeners = new List<DecayListener>();

    public static PlantManager Instance;

    private Coroutine decay;
    
    public void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        decay = StartCoroutine(DecayValues());
    }

    private void OnDestroy()
    {
        if(decay != null)
            StopCoroutine(decay);
    }

    IEnumerator DecayValues()
    {
        while (true)
        {
            waterCurrent -= decayRateWater;
            nutrientCurrent -= decayRateNutrients;
            nutrientsBar.SetFill(nutrientCurrent / nutrientMax);
            waterBar.SetFill(waterCurrent / waterMax);
            
            _listeners.ForEach(listener =>
            {
                    listener.OnDecay();
            });
            
            yield return new WaitForSeconds(1);
        }
    }

    public void AddListener(DecayListener listener)
    {
        _listeners.Add(listener);
    }

    public void AddWater(float waterAdded)
    {
        if (waterAdded + waterCurrent <= waterMax)
        {
            Debug.Log("Adding Water...");
            waterCurrent += waterAdded;
            waterBar.SetFill(waterCurrent / waterMax);
        }
    }

    public void AddNutrient(float nutrientAdded)
    {
        if (nutrientAdded + nutrientCurrent <= nutrientMax)
        {
            nutrientCurrent += nutrientAdded;
            nutrientsBar.SetFill(nutrientCurrent / nutrientMax);
        }
    }
}

public interface DecayListener
{
    public void OnDecay();
}