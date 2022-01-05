using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum ECommand
{
    Up = 'w',
    Left = 'a',
    Down = 's',
    Right = 'd',
}

enum EBattleState
{
    Normal,
    Miss,
    Success,
    Defeat,
    Victory
}

class BattleManager : MonoBehaviour
{
    public EBattleState currentBattleState;
    public GameObject commandWindow;
    public RectTransform commandLine;
    public Sprite[] commandDrawEmpty;
    public Sprite[] commandDrawMiss;
    public Sprite[] commandDrawSuccess;
    public GameObject enemy;

    public ECommand currentCmd
    {
        get { return currentCommand; }
    }
    public int commandCnt
    {
        get { return commandCount; }
    }
    public int currentCmdIdx
    {
        get { return currentCommandIndex; }
    }

    Image commandSprite;
    List<ECommand> commandInput;
    ECommand currentCommand;
    int commandCount;
    int currentCommandIndex;
    int missCount;

    #region Singleton
    private static BattleManager _instance;
    public static BattleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BattleManager>();
                if (_instance == null)
                {
                    Debug.Log("No BattleManager Singleton Object");
                }
            }

            return _instance;
        }
    }

    void Awake()
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

    void Start()
    {
        currentBattleState = EBattleState.Normal;
        commandInput = new List<ECommand>();

        StartCoroutine(Update_FSM());
    }

    IEnumerator Update_FSM()
    {
        while (true)
        {
            // Battle Modes
            if (Player.Instance.currentState == EPlayerState.Battle)
            {
                Battle();
            }
            else if (currentBattleState == EBattleState.Success)
            {
                yield return new WaitForSecondsRealtime(0.3f);
                ExitBattleMode(currentBattleState);
            }
            else if (currentBattleState == EBattleState.Miss)
            {
                yield return new WaitForSecondsRealtime(0.3f);
                ExitBattleMode(currentBattleState);
            }
            else if (currentBattleState == EBattleState.Defeat)
            {
                yield return new WaitForSecondsRealtime(0.3f);
                ExitBattleMode(currentBattleState);
            }

            yield return null;
        }
    }

    void Battle()
    {
        if (currentCommandIndex < commandCount)
        {
            commandSprite = commandLine.GetChild(currentCommandIndex).GetComponent<Image>();
            currentCommand = commandInput[currentCommandIndex];

            // 커맨드 입력
            if (currentCommand == ECommand.Up && Input.GetKeyDown((KeyCode)ECommand.Up))
            {
                currentCommandIndex++;
                commandSprite.sprite = commandDrawSuccess[0];
            }
            else if (currentCommand == ECommand.Down && Input.GetKeyDown((KeyCode)ECommand.Down))
            {
                currentCommandIndex++;
                commandSprite.sprite = commandDrawSuccess[1];
            }
            else if (currentCommand == ECommand.Left && Input.GetKeyDown((KeyCode)ECommand.Left))
            {
                currentCommandIndex++;
                commandSprite.sprite = commandDrawSuccess[2];
            }
            else if (currentCommand == ECommand.Right && Input.GetKeyDown((KeyCode)ECommand.Right))
            {
                currentCommandIndex++;
                commandSprite.sprite = commandDrawSuccess[3];
            }
            // Battle 중에 마우스 차단, 잘못된 커맨드 입력 -> miss 처리
            else if (!Input.GetMouseButtonDown(0) &&
                     !Input.GetMouseButtonDown(1) &&
                     !Input.GetMouseButtonDown(2) && Input.anyKeyDown)
            {
                switch (currentCommand)
                {
                    case ECommand.Up:
                        missCount++;
                        commandSprite.sprite = commandDrawMiss[0];
                        break;
                    case ECommand.Down:
                        missCount++;
                        commandSprite.sprite = commandDrawMiss[1];
                        break;
                    case ECommand.Left:
                        missCount++;
                        commandSprite.sprite = commandDrawMiss[2];
                        break;
                    case ECommand.Right:
                        missCount++;
                        commandSprite.sprite = commandDrawMiss[3];
                        break;
                    default:
                        Debug.Log("Wrong Command");
                        break;
                }
                currentCommandIndex++;
            }
        }
        // 판정
        else if (currentCommandIndex == commandCount && missCount == commandCount)
        {
            Player.Instance.currentState = EPlayerState.Idle;
            currentBattleState = EBattleState.Defeat;
        }
        else if (currentCommandIndex == commandCount && 1 <= missCount)
        {
            Player.Instance.currentState = EPlayerState.Idle;
            currentBattleState = EBattleState.Miss;
        }
        else if (currentCommandIndex == commandCount && missCount == 0)
        {
            Player.Instance.currentState = EPlayerState.Dash;
            currentBattleState = EBattleState.Success;
        }

    }

    public void EnterBattleMode(int _minCommand, int _maxCommand)
    {
        #region Draw & Input Command
        commandCount = Random.Range(_minCommand, _maxCommand + 1);
        for (int i = 0; i < commandCount; i++)
        {
            int commandKey = Random.Range(0, transform.childCount);

            if (transform.GetChild(commandKey).tag == "Up")
            {
                commandInput.Add(ECommand.Up);
            }
            else if (transform.GetChild(commandKey).tag == "Down")
            {
                commandInput.Add(ECommand.Down);
            }
            else if (transform.GetChild(commandKey).tag == "Left")
            {
                commandInput.Add(ECommand.Left);
            }
            else if (transform.GetChild(commandKey).tag == "Right")
            {
                commandInput.Add(ECommand.Right);
            }

            transform.GetChild(commandKey).SetParent(commandLine);
        }
        commandLine.sizeDelta = new Vector2(200 * commandCount, commandLine.sizeDelta.y);
        commandWindow.SetActive(true);
        #endregion
    }

    void ExitBattleMode(EBattleState _state)
    {
        while (0 < commandLine.childCount)
        {
            var command = commandLine.GetChild(0);
            if (command.tag == "Up")
            {
                command.GetComponent<Image>().sprite = commandDrawEmpty[0];
            }
            else if (command.tag == "Down")
            {
                command.GetComponent<Image>().sprite = commandDrawEmpty[1];
            }
            else if (command.tag == "Left")
            {
                command.GetComponent<Image>().sprite = commandDrawEmpty[2];
            }
            else if (command.tag == "Right")
            {
                command.GetComponent<Image>().sprite = commandDrawEmpty[3];
            }
            commandLine.GetChild(0).SetParent(transform);
        }

        if (_state == EBattleState.Success)
        {
            if (enemy.CompareTag("Boss"))
            {
                enemy.GetComponent<Boss>().battleCnt--;
                StartCoroutine(Player.Instance.PlayerKnockback());

                if (enemy.GetComponent<Boss>().battleCnt <= 0)
                {
                    enemy.SetActive(false);
                    currentBattleState = EBattleState.Victory;
                }
            }
            else // if(enemy.CompareTag("Enemy"))
            {
                enemy.SetActive(false);
            }
            currentBattleState = EBattleState.Normal;
        }
        else if (_state == EBattleState.Miss)
        {
            StartCoroutine(Player.Instance.PlayerKnockback());
            Player.Instance.currentState = EPlayerState.Idle;
            currentBattleState = EBattleState.Normal;
        }
        else
        {
            Player.Instance.currentState = EPlayerState.Idle;
        }

        missCount = 0;
        currentCommandIndex = 0;
        commandWindow.SetActive(false);
        commandInput.Clear();
    }
}