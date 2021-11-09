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
    public List<RectTransform> commandList;

    [SerializeField]
    Queue<ECommand> commandQueue;
    bool isBattleMode;
    bool isBattleWin;

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
        commandQueue = new Queue<ECommand>();
    }

    // Update is called once per frame
    void Update()
    {
        // Battle Mode
        if (isBattleMode)
        {
            if (Input.GetKeyDown((KeyCode)ECommand.Down))
            {
                Time.timeScale = 1;
                commandList.Clear();
                commandWindow.SetActive(false);
            }
        }
    }

    public void EnterBattleMode()
    {
        if (true)
        {
            isBattleMode = true;

            #region Draw Command
            int count = Random.Range(4, 5);
            for (int i = 0; i < count; i++)
            {
                int commandKey = Random.Range(0, commands.Length);
                var command = Instantiate(commands[commandKey]).GetComponent<RectTransform>();
                command.SetParent(commandLine);
                commandList.Add(command);

                switch (commandKey)
                {
                    case 0:
                        commandQueue.Enqueue(ECommand.Down);
                        break;
                    case 1:
                        commandQueue.Enqueue(ECommand.Left);
                        break;
                    case 2:
                        commandQueue.Enqueue(ECommand.Up);
                        break;
                    case 3:
                        commandQueue.Enqueue(ECommand.Right);
                        break;
                    default:
                        break;
                }
            }
            commandLine.sizeDelta = new Vector2(200 * count, commandLine.sizeDelta.y);
            commandWindow.SetActive(true);
            #endregion

            Time.timeScale = 0;
        }
    }
}
