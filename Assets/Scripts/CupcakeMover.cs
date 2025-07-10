using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeMover : MonoBehaviour
{
    public float moveHeight = 3f;     // 위아래 이동 높이
    public float moveSpeed = 5f;      // 이동 속도
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // 시작 위치 저장
    }

    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveHeight);
        transform.position = startPos + Vector3.up * offset; // Y축 방향으로 움직이기
    }
}
