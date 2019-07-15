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

    public float healthIndicator = 0.1f;
    private float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Cursor") > 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } 

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

    


    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void UpdateAmmo(string gunName, int bullets)
    {
        ammo.SetText(gunName + " " + bullets);
    }

    public void UpdateHealth(float _health)
    {
        health.SetText("health: " + _health);

        healthOverlay.enabled = true;
        var tempColor = healthOverlay.color;
        tempColor.a = 1f - _health/100; //1f makes it fully visible, 0f makes it fully transparent.
        healthOverlay.color = tempColor;
    }
}
