using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{

    public TextMeshProUGUI ammo;
    public TextMeshProUGUI health;
    public RawImage healthOverlay;

    public GameObject GameOverPanel;
    public GameObject loadButton;
    public AutoSaveScript ass;

    public GameObject YouWinPanel;

    public float healthIndicator = 0.1f;
    private float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            //  DELETE LOCKSTATE WHEN BUILDING !!!
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        GameOverPanel.SetActive(false);
        YouWinPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
      /*  if (Input.GetAxis("Cursor") > 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } */

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (healthOverlay.enabled)
            {
                timer += Time.deltaTime;
                if (timer >= healthIndicator)
                {
                    healthOverlay.enabled = false;
                    timer = 0f;
                }
            }
        }

        if(YouWinPanel.activeSelf == true)
        {
            var tempColor = YouWinPanel.GetComponent<Image>().color;
            tempColor.a += 0.01f; //1f makes it fully visible, 0f makes it fully transparent.
            YouWinPanel.GetComponent<Image>().color = tempColor;
        }

    


    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeScene(int sceneId)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneId);
    }

    public void CloseGameOverPanel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameOverPanel.SetActive(false);
    }

    public void UpdateAmmo(string gunName, int clipBullets, int allBullets)
    {
        if(gunName == "Side arm")
        {
            ammo.SetText(gunName + " " + clipBullets + "/" + "\u221E");
        } else
        {
            ammo.SetText(gunName + " " + clipBullets + "/" + allBullets);
        }
    }

    public void UpdateHealth(float _health)
    {
        health.SetText("health: " + _health);

        healthOverlay.enabled = true;
        var tempColor = healthOverlay.color;
        tempColor.a = 1f - _health/100; //1f makes it fully visible, 0f makes it fully transparent.
        healthOverlay.color = tempColor;

        if(_health <= 0)
        {
            //  DELETE LOCKSTATE WHEN BUILDING !!!
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameOverPanel.SetActive(true);
            Time.timeScale = 0;
            loadButton.SetActive(ass.isSaved);
        }
    }

    public void GainHealth(float _health)
    {
        health.SetText("health: " + _health);

    }

    public void YouWin()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        YouWinPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
