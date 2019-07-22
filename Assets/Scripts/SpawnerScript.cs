using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> prefabs;

    public int level = 2;
    public float spawnDelay = 1;
    List<GameObject> enemys;

    void Start()
    {
        enemys = new List<GameObject>();

        for(int i=0; i < level; i++)
        {
            enemys.AddRange(prefabs);
        }
        StartCoroutine(Spawn());

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Spawn()
    {
        foreach (GameObject enemy in enemys)
        {
            Instantiate(enemy, transform.position, Quaternion.identity, transform);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
