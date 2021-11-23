using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform playerPosition;
    public ParticleSystem chargingEffect;
    public ParticleSystem chargedEffect;
    public DrawLine drawLine;
    public List<Sprite> playerSprite;

    Rigidbody2D m_rigidbody2D;
    LineRenderer lineRenderer;

    readonly float FULL_CHARGE_TIME = 1.3f;

    Vector3 startPosition;
    Vector3 endPosition;

    float currentChargeTime;
    float speed;
    float angle;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        #region 방향(회전) 조정
        angle = Mathf.Atan2(playerPosition.position.y - transform.position.y, playerPosition.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        #endregion

    }
}
