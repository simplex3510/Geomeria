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
    public GameObject[] commandDrawEmpty;
    public Sprite[] commandDrawMiss;
    public Sprite[] commandDrawSuccess;
    public bool battleResult
    {
        get
        {
            if (currentState == EState.win)
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
    EState currentState;
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
        currentState = EState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == EState.win)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(0).transform.SetParent(EnemyManager.Instance.transform);
            ExitBattleMode();
        }
        else if (currentState == EState.defeat)
        {
            ExitBattleMode();
        }

        // Battle Mode
        if (currentState == EState.battle)
        {
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
                            Debug.Log("Ŀ�ǵ� ��� ���� �߻�");
                            break;
                    }
                    currentIndex++;
                    BattleCameraEffect();
                }
            }
            else if (currentIndex == commandCount)
            {
                currentState = EState.win;
            }
        }
    }

    public void EnterBattleMode()
    {
        if (true)
        {
            Time.timeScale = 0;
            currentState = EState.battle;

            #region Draw & Input Command
            commandCount = Random.Range(1, 5);
            for (int i = 0; i < commandCount; i++)
            {
                int commandKey = Random.Range(0, commandDrawEmpty.Length);
                var command = Instantiate(commandDrawEmpty[commandKey]).GetComponent<RectTransform>();
                command.SetParent(commandLine);

                switch (commandKey)
                {
                    case 0:
                        commandInput.Add(ECommand.Up);
                        break;
                    case 1:
                        commandInput.Add(ECommand.Down);
                        break;
                    case 2:
                        commandInput.Add(ECommand.Left);
                        break;
                    case 3:
                        commandInput.Add(ECommand.Right);
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
        currentState = EState.idle;

        for (int i = 0; i < commandLine.childCount; i++)
        {
            Destroy(commandLine.GetChild(i).gameObject);
        }

        // displayDelay = true;
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
