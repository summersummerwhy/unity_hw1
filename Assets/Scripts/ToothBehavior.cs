using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothBehavior : MonoBehaviour
{

    public float maxDistance = 500f; 
    private Vector3 startPos;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position; // 시작 위치 저장
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // float distanceTraveled = Vector3.Distance(transform.position, startPos);

        // if (distanceTraveled > maxDistance)
        // {
        //     Destroy(gameObject);
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle")) {
            Destroy(other.gameObject); // 충돌한 오브젝트 삭제
            Destroy(gameObject); // 충돌한 오브젝트 삭제

            if (player != null)
            {
                player.UpdateHp(+5); 
            }
        }

        if (other.CompareTag("Treat")) {
            Destroy(other.gameObject); // 충돌한 오브젝트 삭제
            Destroy(gameObject); // 충돌한 오브젝트 삭제

            if (player != null)
            {
                player.UpdateHp(+40); 
            }
        }



    }
}
