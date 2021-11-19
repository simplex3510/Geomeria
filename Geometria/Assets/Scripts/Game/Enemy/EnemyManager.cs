using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    GameObject[] children;

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
        // children = GetComponentsInChildren<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (false)
        // {
        //     // 0번은 spawner
        //     for (int i = 1; i < transform.childCount; i++)
        //     {
                
        //     }
        // }
    }
}
