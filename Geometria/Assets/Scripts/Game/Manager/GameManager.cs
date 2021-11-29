using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public RectTransform endWindow;
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
        offset = 1f;
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
            else if (currentGameState == EGameState.End && Player.Instance.currentState == EState.Victory)
            {
                yield return EndState();
            }
            else if (currentGameState == EGameState.End && Player.Instance.currentState == EState.Defeat)
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
        while (true)
        {
            if (BattleTimer.FULL_WIDTH <= width)
            {
                currentGameState = EGameState.Boss;

                EnemyManager.Instance.DisableEnemies();
                enemySpawner.SetActive(false);

                yield break;
            }

            width += BattleTimer.ONE_PERCENT * offset * Time.deltaTime;
            timer.sizeDelta = new Vector2(width, 10);

            yield return null;
        }
    }

    IEnumerator BossState()
    {
        offset = 3f;
        while (true)
        {
            if (width <= 0)
            {
                currentGameState = EGameState.End;
                timer.gameObject.SetActive(false);
                yield break;
            }

            width -= BattleTimer.ONE_PERCENT * offset * Time.deltaTime;
            timer.sizeDelta = new Vector2(width, 10);

            yield return null;
        }
    }

    IEnumerator EndState()
    {
        float width = 0f;
        float height = 0f;
        offset = 150f;

        EnemyManager.Instance.DisableEnemies();
        enemySpawner.SetActive(false);
        endGameSquare.gameObject.SetActive(true);

        while (true)
        {
            // endGameSquare 확대
            if (BattleTimer.FULL_WIDTH <= width)
            {
                offset = 2f;
                Color colorAlpha = endGameSquare.GetComponent<Image>().color;
                StartCoroutine(Rotate());

                // 알파값 감소
                while (true)
                {
                    if (colorAlpha.a <= 0)
                    {
                        endGameSquare.gameObject.SetActive(false);
                        yield return null;
                    }

                    colorAlpha.a -= offset * Time.deltaTime;
                    endGameSquare.GetComponent<Image>().color = colorAlpha;
                    yield return null;
                }
            }

            width  += BattleTimer.ONE_PERCENT * offset * Time.deltaTime;
            height += 10.8f               * offset * Time.deltaTime;
            endGameSquare.sizeDelta = new Vector2(width, height);
            yield return null;
        }
    }

    IEnumerator Rotate()
    {
        float rotateSpeed = 20f;
        player.position = Vector3.zero;
        endWindow.gameObject.SetActive(true);

        while (true)
        {
            player.eulerAngles += new Vector3(0, 0, rotateSpeed * Time.deltaTime);
            map.eulerAngles    -= new Vector3(0, 0, rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnClickRetry()
    {
        SceneManager.LoadScene("GameMain");
    }

    public void OnClickQuit()
    {
        SceneManager.LoadScene("GameTitle");
    }
}
