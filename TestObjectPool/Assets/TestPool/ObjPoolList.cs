using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolList : MonoBehaviour
{
    #region singleton
    private static ObjPoolList _instance = null;
    public static ObjPoolList instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(ObjPoolList)) as ObjPoolList;
                if (!_instance)
                {
                    Debug.LogError(" _instance null");
                    return null;
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogError(" ObjPoolList duplicated.  ");
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public ObjPool m_enemyType1;
    public ObjPool m_enemyType2;

}
