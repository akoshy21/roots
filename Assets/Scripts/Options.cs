using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    private FMOD.Studio.EventInstance SFXVolumeEvent;
    private FMOD.Studio.Bus music;
    private FMOD.Studio.Bus sfx;
    private float musicVolume = 0.5f;
    private float sfxVolume = 0.5f;
    private float masterVolume = 1f;

    public Slider musicSlider, sfxSlider;


    private void Awake()
    {
        sfx = RuntimeManager.GetBus("bus:/Movement");
        music = RuntimeManager.GetBus("bus:/New Group");
        musicSlider.onValueChanged.AddListener (delegate {OnMusicUpdate ();});
        sfxSlider.onValueChanged.AddListener (delegate {OnSfxUpdate ();});

    }


    public void OnMusicUpdate()
    {
        musicVolume = musicSlider.value;
        music.setVolume(musicVolume);
    }
    
    public void OnSfxUpdate()
    {
        sfxVolume = sfxSlider.value;
        sfx.setVolume(sfxVolume);
    }
}
