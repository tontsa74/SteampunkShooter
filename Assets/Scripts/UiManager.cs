using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{

    public TextMeshProUGUI ammo;
    public TextMeshProUGUI health;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Cursor") > 0f)
        {
            Application.Quit();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateAmmo(string gunName, int bullets)
    {
        ammo.SetText(gunName + " " + bullets);
    }

    public void UpdateHealth(float _health)
    {
        health.SetText("health: " + _health);
    }
}
