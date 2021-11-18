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
        StartCoroutine(Move());
    }

    private void OnEnable()
    {
        angle = Mathf.Atan2(playerTransform.position.y - transform.position.y, playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        
        currentState = EState.idle;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (speed <= 0.05f)
            {
                speed = 4f;
                yield return new WaitForSecondsRealtime(0.3f);
            }

            angle = Mathf.Atan2(playerTransform.position.y - transform.position.y, playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            speed = Mathf.Lerp(speed, 0, 0.03f);
            transform.position = Vector3.MoveTowards(enemyTransform.position, playerTransform.position, speed * Time.deltaTime);
            yield return null;
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
