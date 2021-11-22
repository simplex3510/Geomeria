using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    public static readonly float ONE_PERCENT = 19.2f;
    public static readonly int FULL_WIDTH = 1920;
    public RectTransform outline;
    public static float width
    {
        get;
        set;
    }

    float commandMinusOffest;
    float commandPlusOffest;
    

    // Start is called before the first frame update
    void Start()
    {
        commandMinusOffest = 50f / BattleManager.Instance.commandCnt;
        commandPlusOffest = 7f;
        width = FULL_WIDTH;
        outline.sizeDelta = new Vector2(width, 10);
    }

    void OnEnable()
    {
        width = FULL_WIDTH;
        outline.sizeDelta = new Vector2(width, 10);
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (0 <= width)
        {
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

            if (BattleManager.Instance.currentIdx == BattleManager.Instance.commandCnt)
            {
                yield break;
            }

            // 이 부분에서 서로 width 값이 불일치
            width -= (ONE_PERCENT * commandMinusOffest) * Time.deltaTime;
            // Debug.Log($"{gameObject.name}: {(ONE_PERCENT * commandMinusOffest) * Time.deltaTime}");
            Debug.Log($"{gameObject.name}: {width}");
            outline.sizeDelta = new Vector2(width, 10);

            yield return null;
        }
        yield break;
    }

    void ExtendTime()
    {
        Debug.Log($"---------------------------------------------------------------------");
        Debug.Log($"{gameObject.name}: {width}, {UITimer.ONE_PERCENT * commandPlusOffest}");
        UITimer.width += UITimer.ONE_PERCENT * commandPlusOffest;
        if (FULL_WIDTH <= width)
        {
            width = FULL_WIDTH;
        }
        Debug.Log($"{gameObject.name}: {width}");
    }
}
