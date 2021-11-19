using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy[] children;

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
        StartCoroutine(Update_FSM());
    }

    // Update is called once per frame
    IEnumerator Update_FSM()
    {
        while(true)
        {
            if (Player.Instance.currentState == EState.battle)
            {
                yield return StartCoroutine(Pause());
            }
            else
            {

            }
        }
    }

    IEnumerator Pause()
    {
        for (int i = 0; i < transform.childCount; i++)
        {

        }

        yield return null;
    }
}
