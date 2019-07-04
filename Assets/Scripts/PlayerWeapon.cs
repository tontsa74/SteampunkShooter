using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{

    public string name = "Side arm";

    public float damage = 10f;
    public float range = 100f;

    public float fireRate = 0f;
    public int clipSize = 15;

    [HideInInspector]
    public int bullets;

    public float reloadTime = 1f;

    public GameObject graphics;

    public PlayerWeapon()
    {
        bullets = clipSize;
    }
}
