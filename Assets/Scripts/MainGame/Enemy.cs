using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Command
{
    Up = 'w',
    Down = 's',
    Left = 'a',
    Right = 'd',
}

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;

    bool isBattle;
    bool isBattleWin;
    [SerializeField]
    float speed = 3;
    Transform enemyTransform;

    void Start()
    {
        enemyTransform = GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(enemyTransform.position, playerTransform.position, speed * Time.deltaTime);

        if (isBattle)
        {
            if (Input.GetKeyDown((KeyCode)Command.Down))
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

            Time.timeScale = 0;
        }
    }
}
