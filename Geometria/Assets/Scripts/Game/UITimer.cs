using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    public RectTransform outline;
    float width;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        width = 1920;
        outline.sizeDelta = new Vector2(width, 10);
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    IEnumerator Timer()
    {
        while (0 <= width)
        {
            width -= 192 * Time.deltaTime;
            outline.sizeDelta = new Vector2(width, 10);
            yield return null;
        }
        yield break;
    }
}
