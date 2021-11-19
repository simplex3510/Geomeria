using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform playerTransform;

    float speed;
    float angle;
    Rigidbody2D m_rigidbody2D;
    Vector2 direction;
    Transform enemyTransform;

    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();
        StartCoroutine(Move());
    }

    private void OnEnable()
    {
        angle = Mathf.Atan2(playerTransform.position.y - transform.position.y, playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (speed <= 0.07f)
            {
                speed = 4f;
                // yield return new WaitForSecondsRealtime(0.3f);
                yield return null;
            }

            angle = Mathf.Atan2(playerTransform.position.y - transform.position.y, playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            speed = Mathf.Lerp(speed, 0, 0.03f);
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
                gameObject.SetActive(false);
                BattleManager.Instance.enemies.Dequeue();
            }
        }
    }
}
