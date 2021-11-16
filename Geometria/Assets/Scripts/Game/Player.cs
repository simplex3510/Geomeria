using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EState
{
    idle = 0,
    win,
    defeat,
    battle,
    charging,
    charged,
    moving
}

public class Player : MonoBehaviour
{
    public ParticleSystem chargingEffect;
    public ParticleSystem chargedEffect;
    public float speed;
    public EState currentState;

    float currentChargeTime;
    float fullChargeTime = 1f;
    
    Camera cameraMain;
    Rigidbody2D m_rigidbody2D;
    SpriteRenderer spriteRenderer;
    DrawLine drawLine;

    Vector3 startPosition;
    Vector3 movePosition;
    Vector3 currentPosition;

    Vector3 direction;
    Vector3 startPoint;
    Vector3 currentPoint;
    Vector3 endPoint;

    #region Singleton
    private static Player _instance;
    public  static Player Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<Player>();
                if(_instance == null)
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
    }

    void Start()
    {
        cameraMain = Camera.main;
        currentState = EState.idle;
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        drawLine = GetComponentInChildren<DrawLine>();
    }
    #endregion

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentState = EState.charging;

            startPosition = transform.position;

            startPoint = transform.position;
            startPoint.z = -10f;

            chargingEffect.Play();
        }

        if (Input.GetMouseButton(0))
        {
            currentPoint = cameraMain.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = -10f;

            #region 드로우 라인 on
            Vector3 linePoint = (startPoint - currentPoint);
            drawLine.RenderLine(linePoint, linePoint * -1);
            #endregion

            #region 이펙트 및 이미지 로드
            currentChargeTime += Time.deltaTime;
            if (fullChargeTime - 0.15f <= currentChargeTime)
            {
                chargingEffect.Stop();
            }

            if (fullChargeTime <= currentChargeTime)
            {
                // 한 번만 실행
                if (currentState == EState.charged)
                {
                    goto charged;
                }

                chargedEffect.Play();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player_Charged");
            charged: 
                currentState = EState.charged;
            }
            #endregion
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentState = EState.moving;

            endPoint = currentPoint;

            #region 드로우 라인 off
            drawLine.EndLine();
            #endregion

            // 방향 설정 및 이동 거리 설정
            direction = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, -10f, 10f),
                                    Mathf.Clamp(startPoint.y - endPoint.y, -10f, 10f));

            movePosition = direction;           // 이동 해야 할 거리
            direction = direction.normalized;   // 방향 벡터로 설정

            if (fullChargeTime <= currentChargeTime)
            {
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Player");
                m_rigidbody2D.velocity = direction * speed;
                currentChargeTime = 0f;
            }
            else
            {
                currentState = EState.idle;
                chargingEffect.Stop();
                currentChargeTime = 0f;
            }
        }

        currentPosition = transform.position;
        // 이동해야 할 거리와 실재 이동 거리 비교 연산
        if (movePosition.magnitude <= (currentPosition - startPosition).magnitude)
        {
            m_rigidbody2D.velocity = new Vector2(0, 0);
            currentState = EState.idle;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            CameraManager.Instance.isZoom = true;
            m_rigidbody2D.velocity = new Vector2(0, 0);

            switch(currentState)
            {
                case EState.charging:
                case EState.charged:
                case EState.moving:
                    currentState = EState.battle;
                    BattleManager.Instance.EnterBattleMode();
                    break;
                default:
                    // Game Over
                    break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            CameraManager.Instance.isZoom = false;
        }
    }
}