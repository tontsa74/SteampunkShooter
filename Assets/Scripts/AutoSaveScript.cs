using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveScript : MonoBehaviour
{
    public Transform[] autoSavePositions;
    private int destPosition;
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
        void GotoNextPoint() {
        // Returns if no points have been set up
        if (autoSavePositions.Length == 0) {
            return;
        }
            

        // Set the agent to go to the currently selected destination.
        transform.position = autoSavePositions[destPosition].position;
        transform.rotation = autoSavePositions[destPosition].rotation;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPosition = (destPosition + 1) % autoSavePositions.Length;
    }
}
