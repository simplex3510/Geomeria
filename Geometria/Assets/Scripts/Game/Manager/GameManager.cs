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
    Defeat,
    Victory
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

    Color rectColor;

    readonly float ONE_PERCENT = 19.2f;
    readonly int FULL_WIDTH = 1920;

    float timeOffset;
    float record;
    float bestRecord;
    float endRectWidth = 0f;
    float endRectHeight = 0f;
    bool isEnd;
    bool isEndInit;
    bool hasRotate;

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
        timeOffset = 1f;
        currentGameState = EGameState.Normal;
        isEnd = false;
        isEndInit = false;
        hasRotate = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentGameState)
        {
            case EGameState.Normal:
                NormalState();
                break;
            case EGameState.Boss:
                BossState();
                break;
            case EGameState.Defeat:
            case EGameState.Victory:
                EndState();
                break;
            default:
                break;
        }
    }

    void NormalState()
    {
        if (FULL_WIDTH - (ONE_PERCENT * 10) <= width)
        {
            width = FULL_WIDTH - (ONE_PERCENT * 10);
            currentGameState = EGameState.Boss;

            EnemyManager.Instance.DisableEnemies();
            enemySpawner.SetActive(false);

            return;
        }

        if (BattleManager.Instance.currentBattleState == EBattleState.Defeat)
        {
            currentGameState = EGameState.Defeat;
            return;
        }

        width += ONE_PERCENT * timeOffset * Time.deltaTime;
        timer.sizeDelta = new Vector2(width, 10);
    }

    void BossState()
    {

    }

    void EndState()
    {
        if (isEnd == true)
        {
            return;
        }

        if(isEndInit == false)
        {
            timeOffset = 150f;

            EnemyManager.Instance.DisableEnemies();
            endGameSquare.gameObject.SetActive(true);

            Player.Instance.drawLine.EndLine();
            timer.gameObject.SetActive(false);
            enemySpawner.SetActive(false);

            if (currentGameState == EGameState.Defeat)
            {
                gameSetText.text = "Game Over";
            }
            else
            {
                gameSetText.text = "Game Clear";
            }

            isEndInit = true;
        }

        if (FULL_WIDTH <= endRectWidth)
        {
            if(hasRotate == false)
            {
                timeOffset = 2f;
                rectColor = endGameSquare.GetComponent<Image>().color;

                #region ?????? ?????? ??? ??????
                record = (width / FULL_WIDTH) * 100f;
                recordText.text = $"{record:f2}%";

                float bestRecord = PlayerPrefs.GetFloat("BestRecord");
                if (bestRecord <= record)
                {
                    bestRecord = record;
                    PlayerPrefs.SetFloat("BestRecord", bestRecord);
                }
                bestRecordText.text = $"{bestRecord:f2}%";
                #endregion

                Camera.main.transform.position = new Vector3(0, 0, -10f);

                StartCoroutine(Rotate());
                hasRotate = true;
            }

            // ????????? ??????
            if (rectColor.a <= 0)
            {
                endGameSquare.gameObject.SetActive(false);
                isEnd = true;
                return;
            }

            rectColor.a -= timeOffset * Time.deltaTime;
            endGameSquare.GetComponent<Image>().color = rectColor;
        }

        // endGameSquare ??????
        endRectWidth  += ONE_PERCENT * timeOffset * Time.deltaTime;
        endRectHeight += 10.8f * timeOffset * Time.deltaTime;
        endGameSquare.sizeDelta = new Vector2(endRectWidth, endRectHeight);
    }

    public void ExtendTime()
    {
        width += ONE_PERCENT * 2;
        timer.sizeDelta = new Vector2(width, 10);
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
