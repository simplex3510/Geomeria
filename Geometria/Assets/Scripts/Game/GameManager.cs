using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum EGameState
{
    Normal,
    Boss
}

class GameManager : MonoBehaviour
{
    public RectTransform timer;
    public GameObject enemySpawner;
    public EGameState currentGameState;

    public float width
    {
        get;
        set;
    }

    float offset;

    #region GameManager Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    Debug.Log("No GameManager Singleton Object");
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
        offset = 25f;
        currentGameState = EGameState.Normal;
        StartCoroutine(Update_FSM());
    }

    // Update is called once per frame
    IEnumerator Update_FSM()
    {
        while (true)
        {
            if (currentGameState == EGameState.Normal)
            {
                yield return NormalState();
            }
            else if (currentGameState == EGameState.Boss)
            {
                yield return null;
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator NormalState()
    {
        while(true)
        {
            width += UITimer.ONE_PERCENT * offset * Time.deltaTime;
            timer.sizeDelta = new Vector2(width, 10);

            if (UITimer.FULL_WIDTH <= width)
            {
                currentGameState = EGameState.Boss;

                EnemyManager.Instance.DisableEnemies();

                timer.gameObject.SetActive(false);
                enemySpawner.SetActive(false);
            }

            yield return null;
        }
    }

    IEnumerator BossState()
    {
        while (true)
        {
            width += UITimer.ONE_PERCENT * offset * Time.deltaTime;
            timer.sizeDelta = new Vector2(width, 10);

            if (UITimer.FULL_WIDTH <= width)
            {
                currentGameState = EGameState.Boss;

                EnemyManager.Instance.DisableEnemies();

                timer.gameObject.SetActive(false);
                enemySpawner.SetActive(false);
            }

            yield return null;
        }
    }
}
