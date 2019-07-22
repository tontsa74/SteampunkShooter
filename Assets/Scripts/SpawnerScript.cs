using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField]
    public GameObject[] enemys;

    void Start()
    {

        Spawn();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn()
    {
        foreach (GameObject enemy in enemys)
        {
            Instantiate(enemy, transform.position, Quaternion.identity, transform);
        }
    }
}
