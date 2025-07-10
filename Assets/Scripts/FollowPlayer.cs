using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player; // 트럭

    // 각 시점 오프셋 정의
    public Vector3 rearViewOffset = new Vector3(0f, 5f, -7f);          // 2번 - 뒤에서 위
    public Vector3 leftViewOffset = new Vector3(-4f, 3f, -3f);          // 1번 - 왼쪽, 위, 앞
    public Vector3 rightViewOffset = new Vector3(4f, 3f, -3f);          // 3번 - 오른쪽, 위, 앞

    private Vector3 currentOffset;

    void Start()
    {
        currentOffset = rearViewOffset; // 기본은 2번 시점
        transform.position = player.transform.TransformPoint(currentOffset);
    }

    void Update()
    {
        // 번호키 1 → 왼쪽 시점
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentOffset = leftViewOffset;
        }

        // 번호키 2 → 뒤쪽 시점
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentOffset = rearViewOffset;
        }

        // 번호키 3 → 오른쪽 시점
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentOffset = rightViewOffset;
        }
    }

    void LateUpdate()
    {
        // 트럭의 방향 기준으로 오프셋 적용
        transform.position = player.transform.TransformPoint(currentOffset);

        // 트럭을 향하게 카메라 회전
        transform.LookAt(player.transform.position + Vector3.up * 2f); // 약간 위쪽 바라보게
    }
}