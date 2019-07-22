using UnityEngine;

[System.Serializable]
public class SideArm : PlayerWeapon
{

    public SideArm()
    {
        name = "Side arm";
        damage = 10f;
        range = 50f;
        fireRate = 0f;
        clipSize = 15;
        bulletsInClip = clipSize;

        reloadTime = 1f;
        noiseAmount = 15f;

        scopable = false;

    }

    


}
