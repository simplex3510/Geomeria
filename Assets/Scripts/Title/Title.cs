using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void OnClickChallenge()
    {
        SceneManager.LoadScene("GameMain");
    }

    public void OnClickPerkSetting()
    {
        SceneManager.LoadScene("GameTest");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
