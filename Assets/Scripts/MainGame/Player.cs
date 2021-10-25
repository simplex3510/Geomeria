using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;

    Camera m_camera;
    Rigidbody2D m_rigidbody2D;
    
    Vector2 minDistance = new Vector2(-3, -3);
    Vector2 maxDistance = new Vector2(3, 3);
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    void Start()
    {
        m_camera = Camera.main;
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPoint = m_camera.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 1f;
            // Debug.Log("startPoint: " + startPoint.ToString());
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPoint = m_camera.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 1f;
            // Debug.Log("endPoint: " + endPoint.ToString());

            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minDistance.x, maxDistance.x),
                                Mathf.Clamp(startPoint.y - endPoint.y, minDistance.y, maxDistance.y));

            m_rigidbody2D.velocity = force * moveSpeed;
        }
    }
}
