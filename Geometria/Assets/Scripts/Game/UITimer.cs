using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    public RectTransform outline;
    public static float width
    {
        get;
        set;
    }

    float onePercent = 19.2f;

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
        while (0 <= width)
        {
            if(BattleManager.Instance.currentIdx == BattleManager.Instance.commandCnt)
            {
                yield break;
            }

            width -= (onePercent) * Time.deltaTime;
            outline.sizeDelta = new Vector2(width, 10);

            yield return null;
        }
        yield break;
    }
}