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
                PauseGame();
            } else
            {
                ContinueGame();
            }
        }
    }
    public void PauseGame()
    {

        //  DELETE LOCKSTATE WHEN BUILDING !!!
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        print("Pausing");
        paused = true;
        Time.timeScale = 0;
        pausePanel.gameObject.SetActive(true);
    }
    public void ContinueGame()
    {            
        //  DELETE LOCKSTATE WHEN BUILDING !!!
        Cursor.lockState = CursorLockMode.Locked;


        Cursor.visible = false;
        paused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
