using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity; 

public class FMOD_Events : MonoBehaviour
{
    [field: Header("MenuUIClick")]

    [field: SerializeField] public EventReference Click { get; private set; }

    [field: Header("HealthBar Increase")]

    [field: SerializeField] public EventReference Health { get; private set; }

    [field: Header("HealthBar low")]

    [field: SerializeField] public EventReference HealthLow { get; private set; }

    [field: Header("WaterBar Increase")]

    [field: SerializeField] public EventReference Water { get; private set; }

    [field: Header("WaterBar low")]

    [field: SerializeField] public EventReference WaterLow { get; private set; }

    [field: Header("BoneHit")]

    [field: SerializeField] public EventReference BoneHit { get; private set; }

    [field: Header("LeafHit")]

    [field: SerializeField] public EventReference LeafHit { get; private set; }

    [field: Header("AppleHit")]

    [field: SerializeField] public EventReference AppleHit  { get; private set; }

    [field: Header("RootHit")]

    [field: SerializeField] public EventReference RootHit { get; private set; }

    [field: Header("RootBreak")]

    [field: SerializeField] public EventReference RootBreak { get; private set; }

    [field: Header("RockHit")]

    [field: SerializeField] public EventReference RockHit { get; private set; }

    [field: Header("Drain")]

    [field: SerializeField] public EventReference DrainPocket { get; private set; }

    [field: Header("Acorn")]

    [field: SerializeField] public EventReference acornDropped { get; private set; }

    [field: Header("AcornGroundHit")]

    [field: SerializeField] public EventReference acornGroundHit { get; private set; }

    [field: Header("Ambience")]

    [field: SerializeField] public EventReference ambience { get; private set; }

    [field: Header("playerMovement")]

    [field: SerializeField] public EventReference playerMovement { get; private set; }

    [field: Header("Object SFX")]

    [field: SerializeField] public EventReference ObjectInteracted { get; private set; }

    [field: Header("Music")]

    [field: SerializeField] public EventReference Music { get; private set; }

    [field: SerializeField] public EventReference ObjectIdle { get; private set; }

    public static FMOD_Events instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Fmod Events instancein the Scene.");
        }
        instance = this;
    }
}
