using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio; 

public class MenuUI : MonoBehaviour
{ 
    public static bool started=false;
  
    public GameObject menuUI;

    public GameObject acorn;

    private EventInstance Music;

    public void Click()
    {
        AudioManager.instance.PlayOneShot(FMOD_Events.instance.Click, this.transform.position);
        Music = AudioManager.instance.CreateInstance(FMOD_Events.instance.Music);
    }
    private void FixedUpdate()
    {
        UpdateSound();
    }

    private void Start()
    {
    Debug.Log("Scene started");
        if (started)
        {
            menuUI.SetActive(false);
          
            acorn.SetConditionalActive(true);
           
           
            Music = AudioManager.instance.CreateInstance(FMOD_Events.instance.Music);
        }
    }
    public void startGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        started = true;
        UpdateSound();
    }


public void quitGame()
    {
        Application.Quit();
       
    }
   
    private void UpdateSound()
    {

        if (started)
        {

            PLAYBACK_STATE playbackState;
            Music.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                Music.start();
            }
        }
        else
        {
            Music.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
