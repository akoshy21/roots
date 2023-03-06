using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject optionPanel;
    public bool open = false;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        open = !open;
        menuPanel.SetConditionalActive(open);
        if (open)
        {
            OpenPanel();
        }
        else
        {
            ClosePanel();
        }
    }

    private void OpenPanel()
    {
        PlantManager.Instance.PauseGame();
    }
    
    private void ClosePanel()
    {
        PlantManager.Instance.PlayGame();
        optionPanel.SetConditionalActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowOptions()
    {
        optionPanel.SetConditionalActive(true);

    }
}