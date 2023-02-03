using System;
using UnityEngine;

public enum PocketType
{
    Water,
    Nutrient
}

public class Pocket : MonoBehaviour
{
    public float drainRate;
    public float totalValue;

    public PocketType type = PocketType.Water;

    public PlantManager pm;

    private void Start()
    {
        pm = PlantManager.Instance;
    }

    public void DrainPocket()
    {
        if (totalValue > 0)
        {
            totalValue -= drainRate * Time.deltaTime;
            switch (type)
            {
                case PocketType.Water:
                    pm.AddWater(drainRate);
                    break;
                case PocketType.Nutrient:
                    pm.AddWater(drainRate);
                    break;
            }
        }
    }
}