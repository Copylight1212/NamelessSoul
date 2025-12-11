using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject mainPanel;
    public GameObject controlsPanel;
    public GameObject hudUI;  // assign your HUD parent object in Inspector
    
    private bool isPaused = false;

    void Update()
    {
        // Escape pressed?
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        mainPanel.SetActive(false);
        controlsPanel.SetActive(false);
        hudUI.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        mainPanel.SetActive(true);
        controlsPanel.SetActive(false);

        if (hudUI != null)
            hudUI.SetActive(false);  // hide HUD

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPaused = true;
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        if (hudUI != null)
            hudUI.SetActive(true);  // show HUD

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }

    public void OpenControls()
    {
        mainPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        controlsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void QuitGame()
    {
        
        Application.Quit(); // quits built game

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
       
    }
}