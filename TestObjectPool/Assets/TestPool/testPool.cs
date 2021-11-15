using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPool : MonoBehaviour
{
    public void SpawnEnemyType1()
    {
        GameObject obj = ObjPoolList.instance.m_enemyType1.Spawn
            (transform.position, transform.rotation);
    }
    public void SpawnEnemyType2()
    {
        GameObject obj = ObjPoolList.instance.m_enemyType2.Spawn
            (transform.position, transform.rotation);
    }
}
