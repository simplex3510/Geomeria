using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    public static readonly float onePercent = 19.2f;
    public RectTransform outline;
    public static float width
    {
        get;
        set;
    }

    float commandMinusOffest;
    float commandPlusOffest = 10;
    readonly int FULL_WIDTH = 1920;

    // Start is called before the first frame update
    void Start()
    {
        width = 1920;
        outline.sizeDelta = new Vector2(width, 10);
    }

    void OnEnable()
    {
        width = 1920;
        outline.sizeDelta = new Vector2(width, 10);
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        commandMinusOffest = 50 / BattleManager.Instance.commandCnt;
        while (0 <= width)
        {
            if (BattleManager.Instance.currentCmd == ECommand.Up && Input.GetKeyDown((KeyCode)ECommand.Up))
            {
                UITimer.width += UITimer.onePercent * commandPlusOffest;
            }
            else if (BattleManager.Instance.currentCmd == ECommand.Down && Input.GetKeyDown((KeyCode)ECommand.Down))
            {
                UITimer.width += UITimer.onePercent * commandPlusOffest;
            }
            else if (BattleManager.Instance.currentCmd == ECommand.Left && Input.GetKeyDown((KeyCode)ECommand.Left))
            {
                UITimer.width += UITimer.onePercent * commandPlusOffest;
            }
            else if (BattleManager.Instance.currentCmd == ECommand.Right && Input.GetKeyDown((KeyCode)ECommand.Right))
            {
                UITimer.width += UITimer.onePercent * commandPlusOffest;
            }

            if(FULL_WIDTH < width)
            {
                width = FULL_WIDTH;
            }

            if(BattleManager.Instance.currentIdx == BattleManager.Instance.commandCnt)
            {
                yield break;
            }

            width -= (onePercent * commandMinusOffest) * Time.deltaTime;
            outline.sizeDelta = new Vector2(width, 10);

            yield return null;
        }
        yield break;
    }
}
