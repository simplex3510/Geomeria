using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum EGameState
{
    Normal,
    Boss,
    End
}

class GameManager : MonoBehaviour
{
    public RectTransform timer;
    public RectTransform endGameSquare;
    public Transform player;
    public Transform map;
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
                yield return BossState();
            }
            else if (currentGameState == EGameState.End)
            {
                yield return EndState();
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
            if (UITimer.FULL_WIDTH <= width)
            {
                currentGameState = EGameState.Boss;

                EnemyManager.Instance.DisableEnemies();
                enemySpawner.SetActive(false);

                yield break;
            }

            width += UITimer.ONE_PERCENT * offset * Time.deltaTime;
            timer.sizeDelta = new Vector2(width, 10);

            yield return null;
        }
    }

    IEnumerator BossState()
    {
        offset = 5f;
        while (true)
        {
            if (width <= 0)
            {
                currentGameState = EGameState.End;
                timer.gameObject.SetActive(false);
                yield break;
            }

            width -= UITimer.ONE_PERCENT * offset * Time.deltaTime;
            timer.sizeDelta = new Vector2(width, 10);

            yield return null;
        }
    }

    IEnumerator EndState()
    {
        float width = 0f;
        float height = 0f;
        offset = 150f;
        while(true)
        {
            if(UITimer.FULL_WIDTH <= width)
            {
                offset = 1f;
                Color color = endGameSquare.GetComponent<Image>().color;
                while(true)
                {
                    if(color.a <= 0)
                    {
                        break;
                    }

                    color.a -= offset * Time.deltaTime;
                    endGameSquare.GetComponent<Image>().color = color;
                    yield return null;
                }
                break;
            }

            width  += UITimer.ONE_PERCENT * offset * Time.deltaTime;
            height += 10.8f               * offset * Time.deltaTime;
            endGameSquare.sizeDelta = new Vector2(width, height);
            yield return null;
        }

        while (true)
        {
            yield return null;
        }
    }
}
