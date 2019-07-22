using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeScript : MonoBehaviour
{
    private Animator animator;

    bool isScoped = false;

    public float sensitivityMultiplier = 0.1f;
    private float normalSensitivity;

    public GameObject scopeOverlay;
    public GameObject weaponCamera;
    public Camera mainCamera;

    public float scopedFOV = 15f;
    private float normalFOV;

    private PlayerController playerController;
    private WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        weaponManager = GetComponentInParent<WeaponManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2") && !weaponManager.isReloading && weaponManager.GetCurrentWeapon().scopable) {
            animator = weaponManager.GetCurrentWeaponGraphics().animator;
            isScoped = !isScoped;
            animator.SetBool("Scoped", isScoped);

            // scopeOverlay.SetActive(isScoped);

            if (isScoped) {
                StartCoroutine(OnScoped());
            } else {
                OnUnScoped();
            }
        }

        if(weaponManager.isReloading && isScoped || !weaponManager.GetCurrentWeapon().scopable && isScoped)
        {
            isScoped = false;
            animator.SetBool("Scoped", isScoped);
            OnUnScoped();
        }
    }

    IEnumerator OnScoped() {
        yield return new WaitForSeconds(0.15f);

        scopeOverlay.SetActive(true);
        weaponCamera.SetActive(false);
        normalFOV = mainCamera.fieldOfView;
        mainCamera.fieldOfView = scopedFOV;
        normalSensitivity = playerController.GetSensitivity();
        playerController.SetSensitivity(playerController.GetSensitivity() * sensitivityMultiplier);
    }

    void OnUnScoped() {
        scopeOverlay.SetActive(false);
        weaponCamera.SetActive(true);
        mainCamera.fieldOfView = normalFOV;
        playerController.SetSensitivity(normalSensitivity);
    }
}
