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
    float radius = 16f;
    float speed = 1f;
    float runningTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        #region circle move
        runningTime += Time.deltaTime * speed;
        float x = radius * Mathf.Cos(runningTime);
        float y = radius * Mathf.Sin(runningTime);
        transform.position = new Vector2(x, y);
        if(6.3f <= runningTime)
        {
            runningTime = 0f;
        }
        #endregion

        if (Player.Instance.currentState != EState.Battle)
        {
            timeAfterSpawn += Time.deltaTime;
            if (spawnRate <= timeAfterSpawn)
            {
                int enemyIndex = Random.Range(0, 10);
                if (enemys[enemyIndex].gameObject.activeSelf == false)
                {
                    enemys[enemyIndex].transform.position = transform.position;
                    enemys[enemyIndex].SetActive(true);
                }

                spawnRate = Random.Range(spawnRateMin, spawnRateMax);
                timeAfterSpawn = 0;
            }
        }
    }
}
