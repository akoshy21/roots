using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlantManager : MonoBehaviour
{
    public CameraController cam;
    
    public float waterCurrent, waterMax;
    public float nutrientCurrent, nutrientMax;

    public float decayRateWater, decayRateNutrients;

    public CustomFillBar nutrientsBar, waterBar;

    private readonly List<DecayListener> _listeners = new List<DecayListener>();

    public static PlantManager Instance;

    private readonly List<RootController> _activeRoots = new List<RootController>();

    private Coroutine _decay;

    private bool _gameActive = true;
    
    public void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    public void StartGame()
    {
        _decay = StartCoroutine(DecayValues());
    }

    private void OnDestroy()
    {
        if(_decay != null)
            StopCoroutine(_decay);
    }

    IEnumerator DecayValues()
    {
        while (_gameActive)
        {
            waterCurrent -= decayRateWater;
            nutrientCurrent -= decayRateNutrients;
            nutrientsBar.SetFill(nutrientCurrent / nutrientMax);
            waterBar.SetFill(waterCurrent / waterMax);
            
            _listeners.ForEach(listener =>
            {
                    listener.OnDecay();
            });
            
            if(nutrientCurrent <= 0 || waterCurrent <= 0)
                LoseGame();
            
            yield return new WaitForSeconds(1);
        }
    }

    private bool HasActiveRoots()
    {
       return _activeRoots.Exists(root => root.active);
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
        AudioManager.instance.PlayOneShot(FMOD_Events.instance.DrainPocket, this.transform.position);
    }

    public void AddNutrient(float nutrientAdded)
    {
        if (nutrientAdded + nutrientCurrent <= nutrientMax)
        {
            nutrientCurrent += nutrientAdded;
            nutrientsBar.SetFill(nutrientCurrent / nutrientMax);
        }
    }

    public void LoseGame()
    {
        _gameActive = false;
        cam.ScrollToTop();
    }

    public void DeadRoot()
    {
        if (!HasActiveRoots())
        {
            LoseGame();
        }
    }

    public void AddRootController(RootController rc)
    {
        _activeRoots.Add(rc);
    }
}

public interface DecayListener
{
    public void OnDecay();
}