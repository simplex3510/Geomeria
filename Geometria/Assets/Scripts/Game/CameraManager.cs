using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public Camera cameraMain
    {
        get
        {
            return _cameraMain;
        }
    }
    public bool isZoom
    {
        get;
        set;
    }

    Camera _cameraMain;
    bool isCharge = false;
    float fullChargeTime = 1f;
    float currentChargeTime;

    public float currentZoomSize;
    public float zoomIn = 10f;
    public float zoomOut = 13f;
    public float zoomPower = 0.1f;

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
        _cameraMain = Camera.main;
        cameraMain.orthographicSize = 5f;
        CameraZoomEffect(zoomOut, 0.001f);
    }

    private void Update()
    {
        #region Player Camera View
        cameraMain.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        #endregion

        currentZoomSize = cameraMain.orthographicSize;

        #region Charge Camera Effect
        if (Input.GetMouseButton(0))
        {
            currentChargeTime += Time.deltaTime;
            if (fullChargeTime <= currentChargeTime && isCharge == false)
            {
                cameraMain.orthographicSize = currentZoomSize - 1;
                isCharge = true;
            }
        }

        if(isCharge)
        {
            CameraZoomEffect(zoomOut, zoomPower * 0.1f);
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

    public void CameraZoomEffect(float _zoom, float _zoomSpeed)
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