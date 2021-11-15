using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SureExit : MonoBehaviour
{
    float height= 50f;
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        height = Mathf.Lerp(height, 250f, 0.3f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        // Debug.Log(height);
    }

    private void OnDisable()
    {
        height = 50f;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        // Debug.Log(height);
    }
    
    public void OnClickExit()
    {
        Application.Quit();
    }
}
