using UnityEngine;

[System.Serializable]
public class PlayerWeapon1
{

    public string name = "Rail gun";

    public float damage = 20f;
    public float range = 100f;

    public float fireRate = 5f;
    public int clipSize = 8;

    [HideInInspector]
    public int bullets;

    public float reloadTime = 2f;

    public GameObject graphics;

    public PlayerWeapon1()
    {
        bullets = clipSize;
    }
}
