using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;
    public float speed = 1.5f;

    Transform enemyTransform;
    EState currentState;

    void Start()
    {
        currentState = EState.idle;
        enemyTransform = GetComponent<Transform>();
        StartCoroutine(Update_FSM());
    }

    IEnumerator Update_FSM()
    {
        while (true)
        {
            if (currentState == EState.idle)
            {
                yield return new WaitForSecondsRealtime(0.25f);
                currentState = EState.moving;
            }
            else if (currentState == EState.moving)
            {
                yield return StartCoroutine(Move());
                currentState = EState.idle;
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator Move()
    {
        float moveTime = 0f;
        while (true)
        {
            moveTime += Time.deltaTime;
            transform.position = Vector3.MoveTowards(enemyTransform.position, playerTransform.position, speed * Time.deltaTime);

            if (2f <= moveTime)
            {
                yield break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BattleManager.Instance.enemies.Enqueue(this);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Player.Instance.currentState == EState.win)
            {
                gameObject.SetActive(false);
                Player.Instance.currentState = EState.idle;
                BattleManager.Instance.enemies.Dequeue();
            }
        }
    }
}
