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

    public Image nutrientsBar, waterBar;

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
        StopCoroutine(decay);
    }

    IEnumerator DecayValues()
    {
        while (true)
        {
            waterCurrent -= decayRateWater;
            nutrientCurrent -= decayRateNutrients;
            nutrientsBar.fillAmount = nutrientCurrent / nutrientMax;
            waterBar.fillAmount = waterCurrent / waterMax;
            
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
            waterCurrent += waterAdded;
            waterBar.fillAmount = waterCurrent / waterMax;
        }
    }
}

public interface DecayListener
{
    public void OnDecay();
}