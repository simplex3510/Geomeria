using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public GameObject enemy;
    public GameObject destroySquareEffect;
    public ParticleSystem destroyParticleEffect;

    public bool isEffect
    {
        get;
        set;
    }

    void Start()
    {
        isEffect = true;    // 초기값을 true로 하여 처음 시작 시 출력 방지
    }

    void Update()
    {
        // 이펙트는 에너미에 붙어있음
        transform.position = enemy.transform.position;

        // 에너미가 비활성화되고, 이펙트를 출력하지 않았다면
        if(enemy.activeSelf == false && isEffect == false)
        {
            Effect();
            isEffect = true;
        }
    }

    void Effect()
    {
        destroySquareEffect.SetActive(true);                        // 스퀘어 이펙트
        destroyParticleEffect.Play();                               // 파티클 이펙트
        StartCoroutine(CameraManager.Instance.Shake(0.3f, 0.5f));   // 카메라 쉐이크
    }


}
