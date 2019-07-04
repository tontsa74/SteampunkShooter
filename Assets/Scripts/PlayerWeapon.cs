using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{

    public string name = "Rail gun";

    public float damage = 10f;
    public float range = 100f;

    public float fireRate = 10f;
    public int clipSize = 5;

    [HideInInspector]
    public int bullets;

    public GameObject graphics;

    public PlayerWeapon()
    {
        bullets = clipSize;
    }
}
