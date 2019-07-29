using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider collider) {
        if(collider.tag == "Player") {
            AutoSaveScript ass = GetComponentInParent<AutoSaveScript>();
            ass.AutoSave(transform);
        }
        
    }
}
