using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTestManager : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene("GameTitle");
        }
    }
}
