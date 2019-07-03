using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;

    private WeaponManager weaponManager;

    public PlayerWeapon currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (cam == null)
        {
            Debug.LogError("PlayerShoot: Camera not referenced");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit _hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {            
            if(_hit.collider.tag == "Enemy")
            {
                print("PlayerShoot: "+_hit.collider.name);
                EnemyShot(_hit.collider.gameObject, _hit.collider.name, currentWeapon.damage);
            }
        }
    }

    void EnemyShot(GameObject enemy, string collider, float weaponDamage)
    {
        SlenderScript sc = enemy.GetComponentInParent<SlenderScript>();
        sc.TakeDamage(collider, weaponDamage);
    }
}
