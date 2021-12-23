using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy[] enemies;
    public GameObject boss;

    #region EnemyManager Singleton
    private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemyManager>();
                if (_instance == null)
                {
                    Debug.Log("No EnemyManager Singleton Object");
                }
            }

            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion 

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if(GameManager.Instance.currentGameState == EGameState.Boss)
        {
            boss.SetActive(true);
        }
        else if(GameManager.Instance.currentGameState == EGameState.End)
        {
            boss.SetActive(false);
        }
        
    }

    public void DisableEnemies()
    {
        foreach(var enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
