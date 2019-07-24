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

    private CapsuleCollider hearingColl;

    private bool shot = false;


    // Start is called before the first frame update
    void Start()
    {
        hearingColl = GetComponent<CapsuleCollider>();

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
        uiManager.UpdateAmmo(currentWeapon.name,currentWeapon.bulletsInClip, currentWeapon.bulletsAll);

        if(weaponManager.GetCurrentWeaponGraphics().animator.GetCurrentAnimatorStateInfo(0).IsTag("1") && shot)
        {
            shot = false;
        }

    //    if (currentWeapon.fireRate <= 0)
   //     {
    //        if (Input.GetButtonDown("Fire1") && Time.timeScale != 0)
   //         {
   //             Shoot();
   //         }
    //    } else
    //    {
            if(Input.GetButtonDown("Fire1") && Time.timeScale != 0)
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            } else if(Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
  //      }

    }

    void Shoot()
    {
        if(currentWeapon.bulletsInClip <= 0)
        {
            if(currentWeapon.bulletsAll > 0)
            {
                weaponManager.Reload();
            } else
            {
                PlayEmptyClip();
            }
            return;
        }
        if(weaponManager.isReloading || 
            weaponManager.isChanging ||
            shot)
        {
            return;
        }

        currentWeapon.bulletsInClip--;

        PlayMuzzleFlash();
        ThrowShellCasing();
        hearingColl.radius = currentWeapon.noiseAmount;

        shot = true;
        weaponManager.GetCurrentWeaponGraphics().animator.SetTrigger("Shoot");

        RaycastHit _hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))
        {            
            if(_hit.collider.tag == "Enemy")
            {
                EnemyShot(_hit.collider.gameObject, _hit.collider.name, currentWeapon.damage);
            } else
            {
                OnHit(_hit.point, _hit.normal, 10f, _hit.collider.gameObject);
            }
        }

        cam.GetComponent<Recoil>().StartRecoil(0.1f, -15, 2);

    }

    void EnemyShot(GameObject enemy, string collider, float weaponDamage)
    {
        SlenderScript sc = enemy.GetComponentInParent<SlenderScript>();
        sc.TakeDamage(collider, weaponDamage);
    }

    void PlayMuzzleFlash()
    {
        GameObject soundPlayer = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        AudioScript sp = soundPlayer.GetComponent<AudioScript>();
        sp.PlaySound(currentWeapon.shootSound, false, 3f);
        weaponManager.GetCurrentWeaponGraphics().muzzleFlash.Play();
    }

    void OnHit(Vector3 _pos, Vector3 _norm, float showTime, GameObject collider)
    {
        GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentWeaponGraphics().hitEffect, _pos, Quaternion.LookRotation(_norm));
        _hitEffect.transform.SetParent(collider.transform);
        Destroy(_hitEffect, showTime);
    }

    void ThrowShellCasing()
    {
        GameObject _shellCasing = (GameObject)Instantiate(weaponManager.GetCurrentWeaponGraphics().shellCasing, weaponManager.GetCurrentWeaponGraphics().shellPoint);
        Vector3 _casingForce = weaponManager.GetCurrentWeaponGraphics().shellPoint.right * Random.Range(2f,5f);

        _shellCasing.GetComponent<Rigidbody>().AddForce(_casingForce, ForceMode.Impulse);
        Destroy(_shellCasing, 10f);
    }

    public void PlayEmptyClip()
    {
        GameObject soundPlayer = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        AudioScript sp = soundPlayer.GetComponent<AudioScript>();
        sp.PlaySound(currentWeapon.emptyClipSound, false, 3f);
    }
}
