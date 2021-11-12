using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum EState
{
    win = 0,
    defeat,
    battle,
    idle
}

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
    public GameObject[] commandDrawEmpty;
    public Sprite[] commandDrawMiss;
    public Sprite[] commandDrawSuccess;
    public bool battleResult
    {
        get
        {
            if (battleState == EState.win)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    [SerializeField] List<ECommand> commandInput;
    ECommand currentCommand;
    int commandCount;
    int currentIndex;
    EState battleState;
    float delayTime = 0.5f;
    float currentTime =0f;

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
        battleState = EState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (battleState == EState.win)
        {
            Destroy(transform.GetChild(0).gameObject);

            ExitBattleMode();
        }
        else if (battleState == EState.defeat)
        {
            ExitBattleMode();
        }

        // Battle Mode
        if (battleState == EState.battle)
        {
            if (currentIndex < commandCount)
            {
                currentCommand = commandInput[currentIndex];
                if (currentCommand == ECommand.Up && Input.GetKeyDown((KeyCode)ECommand.Up))
                {
                    currentIndex++;
                    BattleCameraEffect();
                }
                else if (currentCommand == ECommand.Down && Input.GetKeyDown((KeyCode)ECommand.Down))
                {
                    currentIndex++;
                    BattleCameraEffect();
                }
                else if (currentCommand == ECommand.Left && Input.GetKeyDown((KeyCode)ECommand.Left))
                {
                    currentIndex++;
                    BattleCameraEffect();
                }
                else if (currentCommand == ECommand.Right && Input.GetKeyDown((KeyCode)ECommand.Right))
                {
                    currentIndex++;
                    BattleCameraEffect();
                    
                }
                else if(Input.anyKeyDown)
                {
                    var missCommand = commandLine.GetChild(currentIndex).GetComponent<Image>();

                    switch (currentCommand)
                    {
                        case ECommand.Left:
                            missCommand.sprite = commandDrawMiss[0];
                            break;
                        case ECommand.Right:
                            missCommand.sprite = commandDrawMiss[1];
                            break;
                        case ECommand.Down:
                            missCommand.sprite = commandDrawMiss[2];
                            break;
                        case ECommand.Up:
                            missCommand.sprite = commandDrawMiss[3];
                            break;
                        default:
                            Debug.Log("커맨드 출력 오류 발생");
                            break;
                    }
                    currentIndex++;
                    BattleCameraEffect();
                }
            }
            else if (currentIndex == commandCount)
            {
                currentTime += Time.deltaTime;
                Debug.Log(currentTime);
                if (delayTime < currentTime)
                {
                    // currentTime = 0;
                    currentIndex = 0;
                    battleState = EState.win;
                }

            }
        }
    }

    public void EnterBattleMode()
    {
        if (true)
        {
            Time.timeScale = 0;
            battleState = EState.battle;

            #region Draw & Input Command
            commandCount = Random.Range(1, 4);
            for (int i = 0; i < commandCount; i++)
            {
                int commandKey = Random.Range(0, commandDrawEmpty.Length);
                var command = Instantiate(commandDrawEmpty[commandKey]).GetComponent<RectTransform>();
                command.SetParent(commandLine);

                switch (commandKey)
                {
                    case 0:
                        commandInput.Add(ECommand.Left);
                        break;
                    case 1:
                        commandInput.Add(ECommand.Right);
                        break;
                    case 2:
                        commandInput.Add(ECommand.Down);
                        break;
                    case 3:
                        commandInput.Add(ECommand.Up);
                        break;
                    default:
                        break;
                }
            }
            commandLine.sizeDelta = new Vector2(200 * commandCount, commandLine.sizeDelta.y);
            commandWindow.SetActive(true);
            #endregion
        }
    }

    void ExitBattleMode()
    {
        for (int i = 0; i < commandLine.childCount; i++)
        {
            Destroy(commandLine.GetChild(i).gameObject);
        }
        battleState = EState.idle;

        currentIndex = 0;
        commandWindow.SetActive(false);
        commandInput.Clear();
        Time.timeScale = 1;
    }

    void BattleCameraEffect()
    {
        CameraManager.Instance.cameraMain.orthographicSize = CameraManager.Instance.currentZoomSize-1;
        CameraManager.Instance.CameraZoomEffect(CameraManager.Instance.currentZoomSize, CameraManager.Instance.zoomPower);
    }

    IEnumerator CommandDisplayDelay()
    {
        yield return new WaitForSecondsRealtime(3f);
    }
}
