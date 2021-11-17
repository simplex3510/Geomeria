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

public class BattleManager : MonoBehaviour
{
    public GameObject commandWindow;
    public RectTransform commandLine;
    public Sprite[] commandDrawEmpty;
    public Sprite[] commandDrawMiss;
    public Sprite[] commandDrawSuccess;
    public Queue<Enemy> enemies;

    List<ECommand> commandInput;
    ECommand currentCommand;
    int commandCount;
    int currentIndex;
    // bool displayDelay = true;

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

    // Start is called before the first frame update
    void Start()
    {
        enemies = new Queue<Enemy>();
        commandInput = new List<ECommand>();
        StartCoroutine(Update_FSM());
    }

    // Update is called once per frame
    IEnumerator Update_FSM()
    {
        Debug.Log("FSM");
        while (true)
        {
            if (Player.Instance.currentState == EState.win)
            {
                yield return StartCoroutine(CWin());
            }
            else if (Player.Instance.currentState == EState.defeat)
            {
                yield return StartCoroutine(CDefeat());
            }
            // Battle Modes
            else if (Player.Instance.currentState == EState.battle)
            {
                yield return StartCoroutine(CBattle());
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator CWin()
    {
        ExitBattleMode();
        yield break;
    }

    IEnumerator CDefeat()
    {
        ExitBattleMode();
        yield break;
    }

    IEnumerator CBattle()
    {
        Debug.Log("CBattle");
        if (currentIndex < commandCount)
        {
            var commandSprite = commandLine.GetChild(currentIndex).GetComponent<Image>();
            currentCommand = commandInput[currentIndex];
            if (currentCommand == ECommand.Up && Input.GetKeyDown((KeyCode)ECommand.Up))
            {
                currentIndex++;
                BattleCameraEffect();
                commandSprite.sprite = commandDrawSuccess[0];
            }
            else if (currentCommand == ECommand.Down && Input.GetKeyDown((KeyCode)ECommand.Down))
            {
                currentIndex++;
                BattleCameraEffect();
                commandSprite.sprite = commandDrawSuccess[1];
            }
            else if (currentCommand == ECommand.Left && Input.GetKeyDown((KeyCode)ECommand.Left))
            {
                currentIndex++;
                BattleCameraEffect();
                commandSprite.sprite = commandDrawSuccess[2];
            }
            else if (currentCommand == ECommand.Right && Input.GetKeyDown((KeyCode)ECommand.Right))
            {
                currentIndex++;
                BattleCameraEffect();
                commandSprite.sprite = commandDrawSuccess[3];

            }
            else if (!Input.GetMouseButtonDown(0) &&
                     !Input.GetMouseButtonDown(1) &&
                     !Input.GetMouseButtonDown(2) && Input.anyKeyDown)
            {
                switch (currentCommand)
                {
                    case ECommand.Up:
                        commandSprite.sprite = commandDrawMiss[0];
                        break;
                    case ECommand.Down:
                        commandSprite.sprite = commandDrawMiss[1];
                        break;
                    case ECommand.Left:
                        commandSprite.sprite = commandDrawMiss[2];
                        break;
                    case ECommand.Right:
                        commandSprite.sprite = commandDrawMiss[3];
                        break;
                    default:
                        Debug.Log("Wrong Command");
                        break;
                }
                currentIndex++;
                BattleCameraEffect();
            }
        }
        else if (currentIndex == commandCount)
        {
            Player.Instance.currentState = EState.win;
            yield return new WaitForSecondsRealtime(0.4f);
            BattleCameraEffect();
        }
        yield return null;
    }

    
    public void EnterBattleMode()
    {
        //Debug.Assert(Player.Instance.currentState == EState.charging ||
        //             Player.Instance.currentState == EState.charged  ||
        //             Player.Instance.currentState == EState.moving   ||
        //             Player.Instance.currentState == EState.battle, "Enter - State Wrong");

        Time.timeScale = 0;
        
        #region Draw & Input Command
        commandCount = Random.Range(1, 5);
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

    void ExitBattleMode()
    {
        //Debug.Assert(Player.Instance.currentState == EState.charging ||
        //             Player.Instance.currentState == EState.charged  ||
        //             Player.Instance.currentState == EState.moving   ||
        //             Player.Instance.currentState == EState.battle, "Exit - State Wrong");

        while (0 < commandLine.childCount)
        {
            var command = commandLine.GetChild(0);
            if(command.tag == "Up")
            {
                command.GetComponent<Image>().sprite = commandDrawEmpty[0];
            }
            else if(command.tag == "Down")
            {
                command.GetComponent<Image>().sprite = commandDrawEmpty[1];
            }
            else if(command.tag == "Left")
            {
                command.GetComponent<Image>().sprite = commandDrawEmpty[2];
            }
            else if(command.tag == "Right")
            {
                command.GetComponent<Image>().sprite = commandDrawEmpty[3];
            }
            commandLine.GetChild(0).SetParent(transform);
        }

        currentIndex = 0;
        commandWindow.SetActive(false);
        commandInput.Clear();
        Time.timeScale = 1;
    }

    void BattleCameraEffect()
    {
        CameraManager.Instance.cameraMain.orthographicSize = CameraManager.Instance.currentZoomSize - 1;
        CameraManager.Instance.CameraZoomEffect(CameraManager.Instance.currentZoomSize, CameraManager.Instance.zoomPower);
    }
}