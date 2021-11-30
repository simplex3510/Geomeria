using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTimer : MonoBehaviour
{
    public static readonly float ONE_PERCENT = 19.2f;
    public static readonly int FULL_WIDTH = 1920;
    public RectTransform outline;
    public float width { get {return currentWidth;}}

    float currentWidth;
    float commandMinusOffest;
    float commandPlusOffest;
    

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<RectTransform>();
        commandPlusOffest = 5f;
        currentWidth = FULL_WIDTH;
        outline.sizeDelta = new Vector2(currentWidth, 10);
    }

    void OnEnable()
    {
        commandMinusOffest = 125f / BattleManager.Instance.commandCnt;
        currentWidth = FULL_WIDTH;
        outline.sizeDelta = new Vector2(currentWidth, 10);
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (true)
        {
            if(currentWidth <= 0)
            {
                Player.Instance.currentState = EState.Defeat;
                yield break;
            }

            if (BattleManager.Instance.currentIdx == BattleManager.Instance.commandCnt)
            {
                yield break;
            }

            if (BattleManager.Instance.currentCmd == ECommand.Up && Input.GetKeyDown((KeyCode)ECommand.Up))
            {
                ExtendTime();
            }
            else if (BattleManager.Instance.currentCmd == ECommand.Down && Input.GetKeyDown((KeyCode)ECommand.Down))
            {
                ExtendTime();
            }
            else if (BattleManager.Instance.currentCmd == ECommand.Left && Input.GetKeyDown((KeyCode)ECommand.Left))
            {
                ExtendTime();
            }
            else if (BattleManager.Instance.currentCmd == ECommand.Right && Input.GetKeyDown((KeyCode)ECommand.Right))
            {
                ExtendTime();
            }

            currentWidth -= (ONE_PERCENT * commandMinusOffest) * Time.deltaTime;
            outline.sizeDelta = new Vector2(currentWidth, 10);

            yield return null;
        }
    }

    void ExtendTime()
    {
        currentWidth += ONE_PERCENT * commandPlusOffest;
        if (FULL_WIDTH <= currentWidth)
        {
            currentWidth = FULL_WIDTH;
        }
    }
}
