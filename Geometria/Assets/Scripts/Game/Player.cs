using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EState
{
    Idle = 0,
    Charging,
    Charged,
    Moving,
    Dash,
    Battle,
    Miss,
    Success,
    Defeat,
    Victory
}

public class Player : MonoBehaviour
{
    public ParticleSystem chargingEffect;
    public ParticleSystem chargedEffect;
    public DrawLine drawLine;
    public Transform targetTransform;
    public List<Sprite> playerSprite;
    public float speed;
    public EState currentState;
    
    readonly float FULL_CHARGE_TIME = 1f;
    float currentChargeTime;
    float angle;
    int dashCount = 4;

    Camera cameraMain;
    Rigidbody2D m_rigidbody2D;
    SpriteRenderer spriteRenderer;

    Vector3 startPosition;
    Vector3 movePosition;
    Vector3 currentPosition;

    Vector3 direction;
    Vector3 startPoint;
    Vector3 currentPoint;
    Vector3 endPoint;

    #region Singleton
    private static Player _instance;
    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
                if (_instance == null)
                {
                    Debug.Log("No Player Singleton Object");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }

        currentState = EState.Idle;
    }
    #endregion

    void Start()
    {
        cameraMain = Camera.main;
        currentState = EState.Idle;
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            currentState != EState.Battle && currentState != EState.Success && currentState != EState.Miss)
        {
            if(currentState != EState.Battle)
            {
                currentState = EState.Charging;
            }
            
            startPosition = transform.position;

            startPoint = transform.position;
            startPoint.z = -10f;

            chargingEffect.Play();
        }

        if (Input.GetMouseButton(0) &&
            currentState != EState.Battle && currentState != EState.Success && currentState != EState.Miss)
        {
            currentPoint = cameraMain.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = -10f;

            #region 방향(회전) 조정
            angle = Mathf.Atan2(transform.position.y - currentPoint.y, transform.position.x - currentPoint.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            #endregion

            #region 드로우 라인 on
            Vector3 linePoint = (startPoint - currentPoint);
            drawLine.RenderLine(linePoint, linePoint * -1);
            #endregion

            #region 이펙트 및 이미지 로드
            currentChargeTime += Time.deltaTime;
            if (FULL_CHARGE_TIME - 0.15f <= currentChargeTime)
            {
                chargingEffect.Stop();
            }

            if (FULL_CHARGE_TIME <= currentChargeTime)
            {
                // 한 번만 실행
                if (currentState == EState.Charged)
                {
                    goto Charged;
                }

                chargedEffect.Play();
                spriteRenderer.sprite = playerSprite[1];
            Charged: 
                currentState = EState.Charged;
            }
            #endregion
        }

        if (Input.GetMouseButtonUp(0) &&
            currentState != EState.Battle && currentState != EState.Success && currentState != EState.Miss)
        {
            currentState = EState.Moving;
            
            endPoint = currentPoint;

            // 방향 설정 및 이동 거리 설정
            direction = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, -10f, 10f),
                                    Mathf.Clamp(startPoint.y - endPoint.y, -10f, 10f));

            movePosition = direction;           // 이동 해야 할 거리
            direction = direction.normalized;   // 방향 벡터로 설정

            if (FULL_CHARGE_TIME <= currentChargeTime)
            {
                spriteRenderer.sprite = playerSprite[0];
                m_rigidbody2D.velocity = direction * speed;
                currentChargeTime = 0f;
            }
            // Charged 전에 Charging을 그만두었을 때
            else
            {
                if (currentState != EState.Battle)
                {
                    currentState = EState.Idle;
                }
                chargingEffect.Stop();
                currentChargeTime = 0f;
            }

            #region 드로우 라인 off
            drawLine.EndLine();
            #endregion
        }

        currentPosition = transform.position;
        // 이동해야 할 거리와 실재 이동 거리 비교 연산
        if (movePosition.magnitude <= (currentPosition - startPosition).magnitude && currentState == EState.Moving)
        {
            movePosition = Vector3.zero;
            startPosition = Vector3.zero;
            m_rigidbody2D.velocity = new Vector2(0, 0);

            currentState = EState.Dash;
        }

        if(currentState == EState.Dash && 0 < dashCount && Physics2D.OverlapCircle(transform.position, 3f))
        {
            Debug.Log("Physics2D.OverlapCircle");
            currentState = EState.Idle;
        }

        // 플레이어 넉백
        if(currentState == EState.Success || currentState == EState.Miss && GameManager.Instance.currentGameState == EGameState.Boss)
        {
            if ((transform.position - targetTransform.position).magnitude <= 3)
            {
                direction = new Vector2(transform.position.x - targetTransform.position.x,
                                        transform.position.y - targetTransform.position.y).normalized;

                m_rigidbody2D.velocity = direction * speed;
                if (3 <= (transform.position - targetTransform.position).magnitude)
                {
                    m_rigidbody2D.velocity = Vector2.zero;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        chargingEffect.Stop();
        drawLine.EndLine();

        if (other.gameObject.CompareTag("Enemy"))
        {
            CameraManager.Instance.isZoom = true;
            m_rigidbody2D.velocity = new Vector2(0, 0);

            switch (currentState)
            {
                case EState.Charging:
                case EState.Charged:
                case EState.Moving:
                case EState.Dash:
                    Debug.Log("배틀");
                    currentState = EState.Battle;
                    BattleManager.Instance.enemies.Add(other.gameObject);
                    BattleManager.Instance.EnterBattleMode(1, 3);
                    break;
                case EState.Idle:
                    // Game Over
                    currentState = EState.Defeat;
                    break;
                default:
                    break;
            }
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            CameraManager.Instance.isZoom = true;
            m_rigidbody2D.velocity = new Vector2(0, 0);

            switch (currentState)
            {
                case EState.Charging:
                case EState.Charged:
                case EState.Moving:
                case EState.Dash:
                    currentState = EState.Battle;
                    BattleManager.Instance.enemies.Add(other.gameObject);
                    BattleManager.Instance.EnterBattleMode(4, 6);
                    break;
                case EState.Idle:
                    // Game Over
                    currentState = EState.Defeat;
                    break;
                default:
                    break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
        {
            CameraManager.Instance.isZoom = false;
        }
    }
}