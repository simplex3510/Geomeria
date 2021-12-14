using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum EGameState
{
    Normal,
    Battle,
    Boss,
    End
}

class GameManager : MonoBehaviour
{
    public RectTransform timer;
    public RectTransform endGameSquare;
    public RectTransform nemo;
    public Transform map;
    public RectTransform endWindow;
    public GameObject enemySpawner;
    public GameObject player;
    public Text recordText;
    public Text bestRecordText;
    public Text gameSetText;
    public EGameState currentGameState;

    public float width
    {
        get;
        set;
    }

    float offset;
    float record;
    float bestRecord;

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

        currentGameState = EGameState.Normal;
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
            switch (currentGameState)
            {
                case EGameState.Normal:
                    yield return NormalState();
                    break;
                case EGameState.Boss:
                    yield return BossState();
                    break;
                case EGameState.End:
                    yield return EndState(Player.Instance.currentState);
                    break;
                default:
                    yield return null;
                    break;
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

            if (Player.Instance.currentState == EState.Defeat)
            {
                currentGameState = EGameState.End;
                yield break;
            }

            width += BattleTimer.ONE_PERCENT * offset * Time.deltaTime;
            timer.sizeDelta = new Vector2(width, 10);

            yield return null;
        }
    }

    IEnumerator BossState()
    {
        offset = 1.5f;
        while (true)
        {
            if (width <= 0)
            {
                currentGameState = EGameState.End;
                yield break;
            }

            if (Player.Instance.currentState == EState.Defeat)
            {
                currentGameState = EGameState.End;
                yield break;
            }
            else if (Player.Instance.currentState == EState.Victory)
            {
                currentGameState = EGameState.End;
                yield break;
            }

            width -= BattleTimer.ONE_PERCENT * offset * Time.deltaTime;
            timer.sizeDelta = new Vector2(width, 10);

            yield return null;
        }
    }

    IEnumerator EndState(EState _state)
    {
        float rectWidth = 0f;
        float rectHeight = 0f;
        offset = 150f;

        EnemyManager.Instance.DisableEnemies();
        endGameSquare.gameObject.SetActive(true);

        Player.Instance.drawLine.EndLine();
        timer.gameObject.SetActive(false);
        enemySpawner.SetActive(false);

        if (_state == EState.Defeat)
        {
            gameSetText.text = "Game Over";
        }
        else
        {
            gameSetText.text = "Game Clear";
        }
        
        while (true)
        {
            // endGameSquare 확대
            if (BattleTimer.FULL_WIDTH <= rectWidth)
            {
                offset = 2f;
                Color colorAlpha = endGameSquare.GetComponent<Image>().color;
                StartCoroutine(Rotate());

                #region 점수 출력 및 저장
                record = (width/BattleTimer.FULL_WIDTH) * 100f;
                recordText.text = $"{record:f2}%";

                float bestRecord = PlayerPrefs.GetFloat("BestRecord");
                if(bestRecord <= record)
                {
                    bestRecord = record;
                    PlayerPrefs.SetFloat("BestRecord", bestRecord);
                }
                bestRecordText.text = $"{bestRecord:f2}%";
                #endregion

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

            rectWidth  += BattleTimer.ONE_PERCENT * offset * Time.deltaTime;
            rectHeight += 10.8f                   * offset * Time.deltaTime;
            endGameSquare.sizeDelta = new Vector2(rectWidth, rectHeight);
            yield return null;
        }
    }

    IEnumerator Rotate()
    {
        float rotateSpeed = 20f;
        player.transform.position = Vector3.zero;
        player.SetActive(false);
        nemo.gameObject.SetActive(true);
        endWindow.gameObject.SetActive(true);

        while (true)
        {
            nemo.eulerAngles += new Vector3(0, 0, rotateSpeed * Time.deltaTime);
            map.eulerAngles  -= new Vector3(0, 0, rotateSpeed * Time.deltaTime);
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
