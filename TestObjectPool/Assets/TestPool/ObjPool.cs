using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool : MonoBehaviour
{
    public List<GameObject> m_lstObject;

    private void Start()
    {
        for(int ix = 0; ix<m_lstObject.Count; ix++)
        {
            m_lstObject[ix].SetActive(false);
        }
    }

    public GameObject Spawn(Vector3 _pos, Quaternion _rot)
    {
        for (int ix = 0; ix < m_lstObject.Count; ix++)
        {
            if(m_lstObject[ix].activeSelf == false)
            {
                m_lstObject[ix].transform.position = _pos;
                m_lstObject[ix].transform.rotation = _rot;
                m_lstObject[ix].SetActive(true);
                return m_lstObject[ix];
            }
        }
        Debug.Log("Spawn null, m_lstObject add !!! ");
        return null;
    }
}
