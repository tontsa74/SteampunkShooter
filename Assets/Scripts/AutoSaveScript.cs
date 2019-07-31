using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoSaveScript : MonoBehaviour
{
    public Transform player;
    public Transform[] autoSavePositions;
    private int destPosition;
    public UiManager uiManager;

    Transform saved;
    public bool isSaved = false;

    public float dropHeight = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonUp("Goto")) {
            GotoNextPoint();
        }
    }

    public void AutoSave(Transform saveTransform) {
        print("saved");
        saved = saveTransform;
        isSaved = true;
    }

    public void LoadSaved() {
        player.position = saved.position + Vector3.up * dropHeight;
        player.rotation = saved.rotation;
    }

    public void LoadSavedDead()
    {
        Time.timeScale = 1;
        uiManager.CloseGameOverPanel();
        player.position = saved.position + Vector3.up * dropHeight;
        player.rotation = saved.rotation;
        player.GetComponentInParent<PlayerController>().GiveHealth(100f);
    }

    void GotoNextPoint() {
    // Returns if no points have been set up
    if (autoSavePositions.Length == 0) {
        return;
    }
            

    // Set the agent to go to the currently selected destination.
    player.position = autoSavePositions[destPosition].position + Vector3.up * dropHeight;
    player.rotation = autoSavePositions[destPosition].rotation;

    // Choose the next point in the array as the destination,
    // cycling to the start if necessary.
    destPosition = (destPosition + 1) % autoSavePositions.Length;
    }
}
