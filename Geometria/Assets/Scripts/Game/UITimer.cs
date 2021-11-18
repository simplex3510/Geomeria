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

            width -= (192 * 2.5f) * Time.deltaTime;
            outline.sizeDelta = new Vector2(width, 10);

            yield return null;
        }
        yield break;
    }
}
