using UnityEngine;

[System.Serializable]
public class RailGun : PlayerWeapon
{

    public RailGun()
    {
        name = "Rail gun";

        damage = 20f;
        range = 100f;

        fireRate = 4f;
        clipSize = 8;

        reloadTime = 2f;

        bulletsInClip = clipSize;
        noiseAmount = 20;

        scopable = true;

    }




}
