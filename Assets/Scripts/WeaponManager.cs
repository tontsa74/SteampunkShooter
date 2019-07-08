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

    public Animator animator;

    private PlayerWeapon currentWeapon;
    public bool isReloading = false;

    public GameObject audioPrefab;


    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(sideArm);

        availableWeapons.Add(sideArm);
        availableWeapons.Add(railGun);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
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
            EquipWeapon(availableWeapons[selectedIndex]);
        }
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        currentWeapon = _weapon;
        _weaponIns.transform.SetParent(weaponHolder);
     //   _weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
        currentWeaponIndex = availableWeapons.IndexOf(_weapon);
        if(weaponHolder.childCount > 1)
        {
            Destroy(weaponHolder.GetChild(0).gameObject);
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

        print("WeaponManager: Reloading");
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentWeapon.bullets = currentWeapon.clipSize;
        isReloading = false;
        animator.SetBool("Reloading", isReloading);

    }
}
