using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMover : MonoBehaviour
{
    public float moveDistance = 10f; // 이동 거리
    public float moveSpeed = 8f; // 이동 속도
    private Vector3 startPos; // 시작 위치

    // Start is called before the first frame update
    void Start()
    {   
        startPos = transform.position; // 시작 위치 저장
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance); // 이동 거리 계산
        transform.position = startPos + transform.forward * offset; // 위치 업데이트
    }
}
