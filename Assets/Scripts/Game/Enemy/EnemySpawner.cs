using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRateMin;
    public float spawnRateMax;
    public GameObject[] enemys;

    float spawnRate = 2f;
    float timeAfterSpawn;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeAfterSpawn += Time.deltaTime;

        if(spawnRate <= timeAfterSpawn)
        {
            if(enemys[Random.Range(0,10)].gameObject.activeSelf == true)

            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
            timeAfterSpawn = 0;
        }
        
    }
}
