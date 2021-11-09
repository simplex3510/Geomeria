using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public bool isZoom
    {
        get;
        set;
    }

    // bool isZoom = false;
    bool isCharge = false;
    float fullChargeTime = 1f;
    float currentChargeTime;
    float zoomIn = 10f;
    float zoomOut = 13f;
    float zoomPower = 0.1f;
    
    Camera cameraMain;

    #region CameraManager Singleton
    private static CameraManager _instance;
    public  static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraManager>();
                if(_instance == null)
                {
                    Debug.Log("No CameraManager Singleton Object");
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private void Start()
    {
        isZoom = false;
        cameraMain = Camera.main;
        cameraMain.orthographicSize = 5f;
        CameraZoomEffect(zoomOut, 0.001f);
    }

    private void Update()
    {
        #region camera view
        cameraMain.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        #endregion

        #region Charge Camera Effect
        if (Input.GetMouseButton(0))
        {
            currentChargeTime += Time.deltaTime;
            if (fullChargeTime <= currentChargeTime && isCharge == false)
            {
                cameraMain.orthographicSize = 12f;
                isCharge = true;
            }
        }

        if(isCharge)
        {
            CameraZoomEffect(zoomOut, 0.01f);
        }

        if(Input.GetMouseButtonDown(0))
        {
            currentChargeTime = 0f;
            isCharge = false;
        }
        #endregion

        if (isZoom)
        {
            CameraZoomEffect(zoomIn, zoomPower);
        }
        else
        {
            CameraZoomEffect(zoomOut, zoomPower);
        }

    }

    void CameraZoomEffect(float _zoom, float _zoomSpeed)
    {
        cameraMain.orthographicSize = Mathf.Lerp(cameraMain.orthographicSize, _zoom, _zoomSpeed);
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