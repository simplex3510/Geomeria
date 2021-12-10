using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody2D m_rigidbody2D;

    float speed;
    float backSpeed;
    float angle;
    Vector2 direction;
    Vector2 currentVelocity;

    void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        angle = Mathf.Atan2(playerTransform.position.y - transform.position.y,
                            playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        StartCoroutine(Update_FSM());
    }

    private void OnDisable()
    {
        StopCoroutine(Update_FSM());
    }

    IEnumerator Update_FSM()
    {
        while (true)
        {
            if (GameManager.Instance.currentGameState == EGameState.Battle)
            {
                yield return StartCoroutine(Pause());
            }
            else if (Player.Instance.currentState == EState.Success)
            {
                yield return StartCoroutine(Resume());
            }
            else if (Player.Instance.currentState == EState.Miss)
            {
                yield return StartCoroutine(EnemyKnockBack());
            }
            else if (Player.Instance.currentState == EState.Dash && Player.Instance.dashCount == 0)
            {
                yield return StartCoroutine(EnemyKnockBack());
                Player.Instance.dashCount = 3;
                Player.Instance.currentState = EState.Idle;
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
        m_rigidbody2D.velocity = currentVelocity;

        yield return null;
    }

    IEnumerator EnemyKnockBack()
    {
        if ((playerTransform.position - transform.position).magnitude <= 10)
        {
            backSpeed = 100f;
            direction = new Vector2(transform.position.x - playerTransform.position.x,
                                    transform.position.y - playerTransform.position.y).normalized;

            m_rigidbody2D.velocity = direction * backSpeed;
            while(true)
            {
                if(10 <= (playerTransform.position - transform.position).magnitude)
                {
                    m_rigidbody2D.velocity = Vector2.zero;
                    yield break;
                }
                yield return null;
            }
        }
        m_rigidbody2D.velocity = currentVelocity;
    }

    IEnumerator Move()
    {
        if (Player.Instance.currentState != EState.Battle)
        {
            if (speed <= 0.25f)
            {
                speed = 5f;
            }

            angle = Mathf.Atan2(playerTransform.position.y - transform.position.y,
                                playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            speed = Mathf.Lerp(speed, 0, 0.05f);
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
            m_rigidbody2D.velocity = Vector2.zero;
        }
    }
}
