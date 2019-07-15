﻿using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{

    public string name;

    public float damage;
    public float range;

    public float fireRate;
    public int clipSize;

    public int bullets;

    public float reloadTime;

    public float noiseAmount;
    public bool scopable;

    public GameObject graphics;

    public AudioClip shootSound;

    public AudioClip reloadSound;

}
