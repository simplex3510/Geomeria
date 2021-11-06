using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum ECommand
{
    Up = 'w',
    Left = 'a',
    Down = 's',
    Right = 'd',
}

public class Enemy : MonoBehaviour
{
    public RectTransform commandLine;
    public GameObject[] commands;
    public List<RectTransform> commandList;

    public Transform playerTransform;
    public float speed = 1.5f;

    bool isBattle;
    bool isBattleWin;
    Transform enemyTransform;

    void Start()
    {
        commandList = new List<RectTransform>();
        enemyTransform = GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(enemyTransform.position, playerTransform.position, speed * Time.deltaTime);

        // Battle Mode
        if (isBattle)
        {
            if (Input.GetKeyDown((KeyCode)ECommand.Down))
            {
                Time.timeScale = 1;
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Enter Battle Mode
            isBattle = true;

            #region Draw Arrow
            int count = Random.Range(0, 4);
            for (int i=0; i<count; i++)
            {
                var command = Instantiate(commands[Random.Range(0, commands.Length)]).GetComponent<RectTransform>();
                command.transform.SetParent(commandLine);
                commandList.Add(command);

                commandLine.sizeDelta = new Vector2(command.sizeDelta.x * count, commandLine.sizeDelta.y);
            }
            #endregion

            Time.timeScale = 0;
        }
    }
}
