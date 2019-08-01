using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject confirmQuitPanel;
    private bool paused = false;

    public GameObject loadButton;
    public GameObject autoSavePositions;
    AutoSaveScript ass;

    void Start()
    {
        pausePanel.SetActive(false);
        confirmQuitPanel.SetActive(false);

        ass = autoSavePositions.GetComponent<AutoSaveScript>();

    }
    void Update()
    {
        if (Input.GetKeyDown("p") || Input.GetButtonDown("Cursor"))
        {
            if (!paused && Time.timeScale == 1)
            {
                PauseGame();
                loadButton.SetActive(ass.isSaved);
            } else if (paused)
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

    public void LoadSaved()
    {
        //  DELETE LOCKSTATE WHEN BUILDING !!!
        Cursor.lockState = CursorLockMode.Locked;


        Cursor.visible = false;
        paused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);

        ass.LoadSaved();
    }

    public void ConfirmQuit()
    {
        confirmQuitPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void CancelQuit()
    {
        confirmQuitPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
}
