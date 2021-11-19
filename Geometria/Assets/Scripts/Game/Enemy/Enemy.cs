using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody2D m_rigidbody2D;

    float speed;
    float angle;
    Vector2 direction;
    Vector2 currentVelocity;

    void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        angle = Mathf.Atan2(playerTransform.position.y - transform.position.y,
                            playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        StartCoroutine(Update_FSM());
    }

    IEnumerator Update_FSM()
    {
        while (true)
        {
            if (Player.Instance.currentState == EState.battle)
            {
                yield return StartCoroutine(Pause());
            }
            else if (Player.Instance.currentState == EState.win || Player.Instance.currentState == EState.defeat)
            {
                yield return StartCoroutine(Resume());
            }
            else
            {
                yield return StartCoroutine(Move());
            }
        }
    }

    IEnumerator Pause()
    {
        currentVelocity = m_rigidbody2D.velocity;
        m_rigidbody2D.velocity = Vector2.zero;

        yield return null;
    }

    IEnumerator Resume()
    {
        if(Player.Instance.currentState == EState.win)
        {
            gameObject.SetActive(false);
            BattleManager.Instance.enemies.Dequeue();
            yield break;
        }

        m_rigidbody2D.velocity = currentVelocity;

        yield return null;
    }

    IEnumerator Move()
    {
        while (Player.Instance.currentState != EState.battle)
        {
            if (speed <= 0.05f)
            {
                speed = 3.5f;
            }

            angle = Mathf.Atan2(playerTransform.position.y - transform.position.y,
                                playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            speed = Mathf.Lerp(speed, 0, 0.01f);
            direction = new Vector2(playerTransform.position.x - transform.position.x,
                                    playerTransform.position.y - transform.position.y).normalized;

            m_rigidbody2D.velocity = direction * speed;
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

            }
        }
    }
}
