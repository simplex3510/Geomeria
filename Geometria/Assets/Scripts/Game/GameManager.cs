using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public RectTransform timer;

    float width;
    float offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // 1분 30초?
        width += UITimer.ONE_PERCENT * offset * Time.deltaTime;
        timer.sizeDelta = new Vector2(width, 10);

        if(UITimer.FULL_WIDTH <= width)
        {
            gameObject.SetActive(false);
        }
    }
}
