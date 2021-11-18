using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;

    float speed;
    float angle;
    Transform enemyTransform;
    EState currentState;

    void Start()
    {
        currentState = EState.idle;
        enemyTransform = GetComponent<Transform>();
        StartCoroutine(Update_FSM());
    }

    private void OnEnable()
    {
        currentState = EState.idle;
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
        while (true)
        {
            angle = Mathf.Atan2(playerTransform.position.y - transform.position.y, playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            speed = Mathf.Lerp(speed, 0, 0.1f);
            transform.position = Vector3.MoveTowards(enemyTransform.position, playerTransform.position, speed * Time.deltaTime);
            yield return null;

            if (speed <= 0.1f)
            {
                speed = 5f;
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
                BattleManager.Instance.enemies.Dequeue();
            }
        }
    }
}
