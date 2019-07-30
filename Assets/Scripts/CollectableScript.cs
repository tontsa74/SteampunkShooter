using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    private bool collected = false;
    // Start is called before the first frame update
    void Start()
    {
     //   transform.Rotate = new Vector3(45, 45, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Collector") && !collected)
        {
            if (this.CompareTag("Health"))
            {
                other.gameObject.GetComponentInParent<PlayerController>().GiveHealth(10f);

            }
            else if(this.CompareTag("Ammo"))
            {
                print("How many times");

                other.gameObject.GetComponentInParent<WeaponManager>().GetCurrentWeapon().bulletsAll += 10;
            }
            Destroy(this.gameObject);
            collected = true;
        }

    }

}
