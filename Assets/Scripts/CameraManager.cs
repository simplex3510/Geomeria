using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    float m_zoomIn = 10f;
    float m_zoomOut = 13f;
    float m_zoomPower = 0.1f;
    public static bool isZoom
    {
        get;
        set;
    }

    Transform playerPosition;
    Camera mainCamera;

    private void Start()
    {
        isZoom = false;
        playerPosition = GetComponent<Transform>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        mainCamera.transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, -10f);
    }

    private void LateUpdate()
    {
        if (isZoom)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, m_zoomIn, m_zoomPower);
        }
        else
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, m_zoomOut, m_zoomPower);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isZoom = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isZoom = false;
        }
    }
}