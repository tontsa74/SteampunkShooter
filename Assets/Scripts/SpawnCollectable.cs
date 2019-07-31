using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollectable : MonoBehaviour
{

    public GameObject ammo;
    public GameObject health;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnAmmoBox()
    {
        if(ammo != null)
        {
            GameObject boxIns = (GameObject)Instantiate(ammo, transform.position + new Vector3(0, 1), Quaternion.Euler(new Vector3(-90, 0, 0)));
        }
        //  boxIns.GetComponent<Collider>().enabled = false;
        //    boxIns.GetComponent<Collider>().enabled = true;

    }

    public void SpawnHealthBox()
    {
        if(health != null)
        {
            GameObject boxIns = (GameObject)Instantiate(health, transform.position + new Vector3(0, 1), Quaternion.identity);

        }
        //    boxIns.GetComponent<Collider>().enabled = false;
        //  boxIns.GetComponent<Collider>().enabled = true;
    }

}
