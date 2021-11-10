using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject[] commands;
    public RectTransform commandLine;
    public bool isBattleResult
    {
        get
        {
            return isBattleWin;
        }
    }

    [SerializeField] List<RectTransform> commandList;
    [SerializeField] List<ECommand> commandInput;
    ECommand currentCommand;
    int lastIndex;
    int currentIndex;
    bool isBattleWin = false;
    bool isBattleMode;

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

    }

    // Update is called once per frame
    void Update()
    {
        if (isBattleWin == true)
        {
            commandWindow.SetActive(false);
            ExitBattleMode();
            Time.timeScale = 1;
        }

        // Battle Mode
        if (isBattleMode)
        {
            if (currentIndex < lastIndex)
            {
                currentCommand = commandInput[currentIndex];
                if (currentCommand == ECommand.Up && Input.GetKeyDown((KeyCode)ECommand.Up))
                {
                    currentIndex++;
                }
                else if (currentCommand == ECommand.Down && Input.GetKeyDown((KeyCode)ECommand.Down))
                {
                    currentIndex++;
                }
                else if (currentCommand == ECommand.Left && Input.GetKeyDown((KeyCode)ECommand.Left))
                {
                    currentIndex++;
                }
                else if (currentCommand == ECommand.Right && Input.GetKeyDown((KeyCode)ECommand.Right))
                {
                    currentIndex++;
                }
            }
            else if (currentIndex == lastIndex)
            {
                isBattleWin = true;
            }
        }
    }

    public void EnterBattleMode()
    {
        if (true)
        {
            Time.timeScale = 0;
            isBattleMode = true;

            #region Draw & Input Command
            lastIndex = Random.Range(4, 5);
            for (int i = 0; i < lastIndex; i++)
            {
                int commandKey = Random.Range(0, commands.Length);
                var command = Instantiate(commands[commandKey]).GetComponent<RectTransform>();
                command.SetParent(commandLine);
                commandList.Add(command);

                switch (commandKey)
                {
                    case 0:
                        commandInput.Add(ECommand.Down);
                        break;
                    case 1:
                        commandInput.Add(ECommand.Left);
                        break;
                    case 2:
                        commandInput.Add(ECommand.Up);
                        break;
                    case 3:
                        commandInput.Add(ECommand.Right);
                        break;
                    default:
                        break;
                }
            }
            commandLine.sizeDelta = new Vector2(200 * lastIndex, commandLine.sizeDelta.y);
            commandWindow.SetActive(true);
            #endregion
        }
    }

    void ExitBattleMode()
    {
        for(int i=0; i<commandLine.childCount; i++)
        {
            Destroy(commandLine.GetChild(i).gameObject);
        }

        currentIndex = 0;
        commandList.Clear();
        commandInput.Clear();
    }
}
