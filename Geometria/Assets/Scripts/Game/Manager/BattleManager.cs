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

class BattleManager : MonoBehaviour
{
    public GameObject commandWindow;
    public RectTransform commandLine;
    public Sprite[] commandDrawEmpty;
    public Sprite[] commandDrawMiss;
    public Sprite[] commandDrawSuccess;
    public List<GameObject> enemies;
    public ECommand currentCmd
    {
        get { return currentCommand; }
    }
    public int commandCnt
    {
        get { return commandCount; }
    }
    public int currentIdx
    {
        get { return currentIndex; }
    }

    List<ECommand> commandInput;
    ECommand currentCommand;
    int commandCount;
    int currentIndex;
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

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
        commandInput = new List<ECommand>();
        StartCoroutine(Update_FSM());
    }

    // Update is called once per frame
    IEnumerator Update_FSM()
    {
        while (true)
        {
            if (Player.Instance.currentState == EState.Success)
            {
                yield return new WaitForSecondsRealtime(0.3f);
                BattleCameraEffect();
                yield return StartCoroutine(CWin());
            }
            else if (Player.Instance.currentState == EState.Miss)
            {
                yield return new WaitForSecondsRealtime(0.3f);
                BattleCameraEffect();
                yield return StartCoroutine(CMiss());
            }
            else if (Player.Instance.currentState == EState.Defeat)
            {
                yield return StartCoroutine(CDefeat());
            }
            // Battle Modes
            else if (Player.Instance.currentState == EState.Battle)
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
        ExitBattleMode(EState.Success);
        yield break;
    }

    IEnumerator CMiss()
    {
        ExitBattleMode(EState.Miss);
        yield break;
    }

    IEnumerator CDefeat()
    {
        ExitBattleMode(EState.Defeat);
        yield break;
    }

    IEnumerator CBattle()
    {
        if (currentIndex < commandCount)
        {
            var commandSprite = commandLine.GetChild(currentIndex).GetComponent<Image>();
            currentCommand = commandInput[currentIndex];

            // 커맨드 입력
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
            // Battle 중에 마우스 차단 && 잘못된 커맨드 입력 -> miss 처리
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
                currentIndex++;
                BattleCameraEffect();
            }
        }
        else if (currentIndex == commandCount && missCount == commandCount)
        {
            Player.Instance.currentState = EState.Defeat;
            yield return new WaitForSecondsRealtime(0.3f);
            BattleCameraEffect();
        }
        else if (currentIndex == commandCount && 1 <= missCount)
        {
            Player.Instance.currentState = EState.Miss;
            yield return new WaitForSecondsRealtime(0.3f);
            BattleCameraEffect();
        }
        else if (currentIndex == commandCount && missCount == 0)
        {
            Player.Instance.currentState = EState.Success;
            yield return new WaitForSecondsRealtime(0.3f);
            BattleCameraEffect();
        }
        yield return null;
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

    void ExitBattleMode(EState _state)
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

        if(_state == EState.Success)
        {
            for (int i = 0; enemies.Count != 0; i++)
            {
                var enemy = enemies[i];
                enemies.RemoveAt(i);
                if (enemy.CompareTag("Boss"))
                {
                    if (enemy.GetComponent<Boss>().battleCnt == 0)
                    {
                        enemy.transform.parent.gameObject.SetActive(false);
                    }
                    else
                    {
                        enemy.GetComponent<Boss>().battleCnt--;
                    }
                }
                else // if(enemy.CompareTag("Enemy"))
                {
                    enemy.SetActive(false);
                }
                Player.Instance.currentState = EState.Dash;
            }
        }
        else if(_state == EState.Miss)
        {
            for (int i = 0; enemies.Count != 0; i++)
            {
                var enemy = enemies[i];
                enemies.RemoveAt(i);
            }
            Player.Instance.currentState = EState.Dash;
        }
        else if(_state == EState.Defeat)
        {
            for (int i = 0; enemies.Count != 0; i++)
            {
                var enemy = enemies[i];
                enemies.RemoveAt(i);
            }
        }

        missCount = 0;
        currentIndex = 0;
        commandWindow.SetActive(false);
        commandInput.Clear();
    }

    void BattleCameraEffect()
    {
        CameraManager.Instance.cameraMain.orthographicSize = CameraManager.Instance.currentZoomSize - 1;
        CameraManager.Instance.CameraZoomEffect(CameraManager.Instance.currentZoomSize, CameraManager.Instance.zoomPower);
    }

}