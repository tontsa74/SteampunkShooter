using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    [SerializeField]
    private PlayerWeapon1 secondaryWeapon;

    [SerializeField]
    private Transform weaponHolder;

    public Animator animator;

    private PlayerWeapon currentWeapon;
    private bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);
        _weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
    }

    public void Reload()
    {
        if(isReloading)
        {
            return;
        }

        StartCoroutine(Reload_Coroutine());
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
