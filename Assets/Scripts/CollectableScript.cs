using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    private bool collected = false;
    Vector3 tempPos = new Vector3();
    Vector3 posOffset = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.CompareTag("Health"))
        {
            transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
        } else
        {
            transform.Rotate(new Vector3(0, 0, 30) * Time.deltaTime);
        }
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 0.5f) * 0.3f;

        transform.position = tempPos;
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
                other.gameObject.GetComponentInParent<WeaponManager>().GetRailGun().bulletsAll += 10;
            }
            Destroy(this.gameObject);
            collected = true;
        }

    }

}
