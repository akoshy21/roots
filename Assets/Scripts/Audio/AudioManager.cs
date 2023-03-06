using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;

    private List<StudioEventEmitter> eventEmitters;

    private EventInstance ambienceEventInstance;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manger in the scene.");
        }
        instance = this;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }
    
    private void start()
    {
        IntializeAmbience(FMOD_Events.instance.ambience);
       
    }
    
    private void IntializeAmbience(EventReference ambienceEventReference) 
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start(); 
    }
    public void SetAmbienceParameter (string ParameterName, float ParameterValue) 
    
    {
        ambienceEventInstance.setParameterByName(ParameterName,ParameterValue);  
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }
    
    private void CleanUp()
    {
//stop realese instances 
    foreach(EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();

        }
    foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }    
    
    }


    private void OnDestroy()
    {
        CleanUp();
    }
}
