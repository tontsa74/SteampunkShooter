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
        GameObject boxIns = (GameObject)Instantiate(ammo, transform.position + new Vector3(0, 1), Quaternion.EulerRotation(new Vector3(45,45,45)));
        //  boxIns.GetComponent<Collider>().enabled = false;
        //    boxIns.GetComponent<Collider>().enabled = true;

    }

    public void SpawnHealthBox()
    {
        GameObject boxIns = (GameObject)Instantiate(health, transform.position + new Vector3(0, 1), Quaternion.EulerRotation(new Vector3(45, 45, 45)));
    //    boxIns.GetComponent<Collider>().enabled = false;
      //  boxIns.GetComponent<Collider>().enabled = true;
    }

}
