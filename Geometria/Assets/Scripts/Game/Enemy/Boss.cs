using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBossState
{
    Idle = 0,
    Charging,
    Charged,
    Moving,
    Dash,
    Battle,
}

public class Boss : MonoBehaviour
{
    public Transform playerPosition;
    public ParticleSystem chargingEffect;
    public ParticleSystem chargedEffect;
    public DrawLine drawLine;
    public Sprite[] bossSprites;
    public float battleCnt
    {
        get { return battleCount; }
        set { battleCount = value; }
    }

    readonly float FULL_CHARGE_TIME = 1.2f;

    SpriteRenderer spriteRenderer;
    Rigidbody2D m_rigidbody2D;
    Vector3 startPosition;
    Vector3 currentPosition;
    Vector3 endPosition;
    Vector3 movePosition;
    Vector3 linePoint;
    Vector3 direction;
    EBossState currentState;

    float currentChargeTime;
    float speed = 100f;
    float angle;
    float battleCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();

        currentState = EBossState.Charging;
        StartCoroutine(Update_FSM());
    }

    // Update is called once per frame
    IEnumerator Update_FSM()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        while (true)
        {
            Debug.Log("FSM");
            switch (currentState)
            {
                case EBossState.Idle:
                    yield return Pause();
                    break;
                case EBossState.Charging:
                    yield return Charging();
                    break;
                case EBossState.Charged:
                    currentState = EBossState.Moving;
                    break;
                case EBossState.Moving:
                    yield return Move();
                    break;
                default:
                    yield return null;
                    break;
            }
        }
    }

    IEnumerator Pause()
    {
        while (true)
        {
            if (currentState != EBossState.Idle)
            {
                StartCoroutine(BossKnockback());
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator Charging()
    {
        chargingEffect.Play();
        startPosition = transform.position;

        while (true)
        {
            if (currentState != EBossState.Charging)
            {
                chargingEffect.Stop();
                yield break;
            }

            #region 방향(회전) 조정
            angle = Mathf.Atan2(playerPosition.position.y - transform.position.y, playerPosition.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            #endregion

            #region 드로우 라인 on
            linePoint = (playerPosition.position - transform.position);
            drawLine.RenderLine(linePoint, linePoint * -1);
            #endregion

            currentChargeTime += Time.deltaTime;
            if (FULL_CHARGE_TIME - 0.15f <= currentChargeTime)
            {
                chargingEffect.Stop();
            }

            if (FULL_CHARGE_TIME <= currentChargeTime)
            {
                // 한 번만 실행
                if (currentState == EBossState.Charged)
                {
                    yield return new WaitForSecondsRealtime(0.5f);
                    yield break;
                }

                endPosition = playerPosition.position;
                chargedEffect.Play();
                spriteRenderer.sprite = bossSprites[1];
                currentState = EBossState.Charged;
            }

            yield return null;
        }
    }

    IEnumerator Move()
    {
        // 방향 설정 및 이동 거리 설정
        direction = new Vector2(Mathf.Clamp(endPosition.x - transform.position.x, -10f, 10f),
                                Mathf.Clamp(endPosition.y - transform.position.y, -10f, 10f));

        movePosition = direction;           // 이동 해야 할 거리
        direction = direction.normalized;   // 방향 벡터로 설정

        if (FULL_CHARGE_TIME <= currentChargeTime)
        {
            spriteRenderer.sprite = bossSprites[0];
            m_rigidbody2D.velocity = direction * speed;
            currentChargeTime = 0f;
        }

        #region 드로우 라인 off
        drawLine.EndLine();
        #endregion

        while (true)
        {
            if (currentState != EBossState.Moving)
            {
                yield break;
            }

            currentPosition = transform.position;
            // 이동해야 할 거리와 실재 이동 거리 비교 연산
            if (movePosition.magnitude <= (currentPosition - startPosition).magnitude)
            {
                m_rigidbody2D.velocity = new Vector2(0, 0);
                break;
            }
            yield return null;
        }

        currentState = EBossState.Charging;

        yield return new WaitForSecondsRealtime(1.5f);
    }

    IEnumerator BossKnockback()
    {
        float backSpeed = 100f;
        float duration = 0f;
        direction = new Vector2(transform.position.x - playerPosition.position.x,
                                transform.position.y - playerPosition.position.y).normalized;

        m_rigidbody2D.velocity = direction * backSpeed;
        while (true)
        {
            duration += Time.deltaTime;
            if (0.12f <= duration)
            {
                m_rigidbody2D.velocity = Vector2.zero;
                yield break;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_rigidbody2D.velocity = Vector2.zero;
            currentChargeTime = 0f;
            drawLine.EndLine();
            currentState = EBossState.Idle;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentState = EBossState.Charging;
        }
    }
}
