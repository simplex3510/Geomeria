using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab;
    public float spawnRateMin;
    public float spawnRateMax;

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
            timeAfterSpawn = 0;

            var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            enemy.transform.SetParent(null);
        }
        
    }
}
