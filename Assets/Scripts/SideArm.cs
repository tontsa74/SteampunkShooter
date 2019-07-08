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
        bullets = clipSize;

        reloadTime = 1f;

        scopable = false;

    }

    


}
