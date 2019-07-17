using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;
    private bool paused = false;

    void Start()
    {
        pausePanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown("p") || Input.GetButtonDown("Cursor"))
        {
            if (!paused)
            {
             //   Cursor.lockState = CursorLockMode.None;
                PauseGame();
            } else
            {
                ContinueGame();
           //     Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    public void PauseGame()
    {
        Cursor.visible = true;
        print("Pausing");
        paused = true;
        Time.timeScale = 0;
        pausePanel.gameObject.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    }
    public void ContinueGame()
    {
        Cursor.visible = false;
        paused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        //enable the scripts again
    }
}
