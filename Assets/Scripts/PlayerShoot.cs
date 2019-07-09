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

    public UiManager uiManager;


    public GameObject audioPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);

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
        uiManager.UpdateAmmo(currentWeapon.name,currentWeapon.bullets);


        if (currentWeapon.fireRate <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        } else
        {
            if(Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            } else if(Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }

    }

    void Shoot()
    {
        if(currentWeapon.bullets <= 0)
        {
            weaponManager.Reload();
            return;
        }
        if(weaponManager.isReloading)
        {
            return;
        }

        currentWeapon.bullets--;
        print("PlayerShoot: Remaining bullets " + currentWeapon.bullets);

        cam.GetComponent<Recoil>().StartRecoil(0.1f, -15, 2);

        GameObject soundPlayer = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        AudioScript sp = soundPlayer.GetComponent<AudioScript>();
        sp.PlaySound(currentWeapon.shootSound, false, 3f);
        PlayMuzzleFlash();


        RaycastHit _hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {            
            if(_hit.collider.tag == "Enemy")
            {
                print("PlayerShoot: "+_hit.collider.name);
                EnemyShot(_hit.collider.gameObject, _hit.collider.name, currentWeapon.damage);
            } else
            {
                OnHit(_hit.point, _hit.normal, 2f);
            }
        }
    }

    void EnemyShot(GameObject enemy, string collider, float weaponDamage)
    {
        SlenderScript sc = enemy.GetComponentInParent<SlenderScript>();
        sc.TakeDamage(collider, weaponDamage);
    }

    void PlayMuzzleFlash()
    {
            weaponManager.GetCurrentWeaponGraphics().muzzleFlash.Play();
    }

    void OnHit(Vector3 _pos, Vector3 _norm, float showTime)
    {
        GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentWeaponGraphics().hitEffect, _pos, Quaternion.LookRotation(_norm));
        Destroy(_hitEffect, showTime);
    }
}
