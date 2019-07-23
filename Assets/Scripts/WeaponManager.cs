using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private SideArm sideArm;

    [SerializeField]
    private RailGun railGun;

    private List<PlayerWeapon> availableWeapons = new List<PlayerWeapon>();
    private int currentWeaponIndex;

    [SerializeField]
    private Transform weaponHolder;

    private Animator animator;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;
    public bool isReloading = false;
    public bool isChanging = false;


    public GameObject audioPrefab;


    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(sideArm);

        availableWeapons.Add(railGun);
        availableWeapons.Add(sideArm);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentWeaponGraphics()
    {
        return currentGraphics;
    }

    public void Update()
    {
        PlayerWeapon lastSelectedWeapon = currentWeapon;
        int selectedIndex = currentWeaponIndex;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(selectedIndex < availableWeapons.Count-1)
            {
                selectedIndex++;

            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedIndex > 0)
            {
                selectedIndex--;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (selectedIndex != currentWeaponIndex)
        {
            // EquipWeapon(availableWeapons[selectedIndex]);
            StartCoroutine(ChangingWeapon_Coroutine(availableWeapons[selectedIndex]));
        }
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        currentWeapon = _weapon;
        currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
        _weaponIns.transform.SetParent(weaponHolder);
     //   _weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
        currentWeaponIndex = availableWeapons.IndexOf(_weapon);

        animator = currentGraphics.animator;
        if (weaponHolder.childCount > 1)
        {
            for(int i=0; i<weaponHolder.childCount-1; i++)
            {
                Destroy(weaponHolder.GetChild(i).gameObject);
            }

        }
    }

    public void Reload()
    {
        if(isReloading)
        {
            return;
        }

        StartCoroutine(Reload_Coroutine());
        GameObject soundPlayer = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        AudioScript sp = soundPlayer.GetComponent<AudioScript>();
        sp.PlaySound(currentWeapon.reloadSound, false, 3f);
    }

    private IEnumerator Reload_Coroutine()
    {
        isReloading = true;
        animator.SetBool("Reloading", isReloading);

        yield return new WaitForSeconds(currentWeapon.reloadTime);
        if(currentWeapon.bulletsAll >= currentWeapon.clipSize - currentWeapon.bulletsInClip)
        {
            currentWeapon.bulletsAll -= currentWeapon.clipSize - currentWeapon.bulletsInClip;
            currentWeapon.bulletsInClip = currentWeapon.clipSize;
        } else if(currentWeapon.bulletsAll > 0)
        {
            currentWeapon.bulletsInClip = currentWeapon.bulletsAll;
            currentWeapon.bulletsAll = 0;
        }

        isReloading = false;
        animator.SetBool("Reloading", isReloading);

    }

    private IEnumerator ChangingWeapon_Coroutine(PlayerWeapon _weapon)
    {
        isChanging = true;
        // animator.SetBool("Changing", isChanging);
        animator.SetBool("Reloading", isChanging);

        yield return new WaitForSeconds(1.5f);
        EquipWeapon(_weapon);
        currentWeapon.bulletsInClip = currentWeapon.clipSize;
        isChanging = false;
        // animator.SetBool("Changing", isChanging);
        animator.SetBool("Reloading", isChanging);


    }
}
