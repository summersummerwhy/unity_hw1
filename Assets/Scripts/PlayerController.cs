using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerController : MonoBehaviour
{
    // basic variables
    public float acceleration = 20f;
    public float deceleration = 20f;
    public float maxSpeed = 300f;
    public float turnSpeed = 60f;

    private float currentSpeed = 0f;
    private float forwardInput;
    private float horizontalInput;

    // Rigidbody
    private Rigidbody rb;

    // 지면 감지용
    private bool isGrounded;
    private Vector3 lastGroundNormal = Vector3.up; // 🔧 기본은 위로

    private float airTime = 0f;
    public float maxAirTime = 4f; // 이 이상 떠 있으면 탈선으로 간주



    // UI
    public TextMeshProUGUI gameOverText;
    private int hp;
    public TextMeshProUGUI hpText;

    public float jumpForce = 40000f;
    public float forwardJumpFactor = 1.8f; 

    // Projectile variables
    public GameObject projectilePrefab;
    private float projectileSpeed = 30f;
    public float forwardOffset = 6f;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.7f, 0f); // 🔧 중심을 아래로
        hp = 0;
        UpdateHp(100);
    }

    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 트럭 앞쪽으로 약간 떨어진 위치 계산
            Vector3 spawnPosition = transform.position + transform.forward * forwardOffset + Vector3.up * 1.5f;

            // 발사체 생성 (트럭과 같은 방향으로 회전)
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.LookRotation(transform.forward));

            // Rigidbody에 속도 적용
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = transform.forward * projectileSpeed;
            }
        }

            if (Input.GetKeyDown(KeyCode.J))
        {
            Jump(); // 여기에서 점프 키 처리!
        }
    }


    void Jump()
    {

        // 위쪽 + 앞쪽 방향 조합
        Vector3 jumpDirection = (Vector3.up + transform.forward * forwardJumpFactor).normalized;
        rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        // Debug.Log($"점프! 방향: {jumpDirection}, 힘: {jumpForce}");
        


    }


    void GameOver()
    {

        // Debug.Log("GAME OVER! 트랙에서 떨어졌습니다.");

        // UI에 메시지 표시
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER!!!";
            gameOverText.gameObject.SetActive(true);
        }

        // 2초 뒤 씬 리셋
        Invoke("RestartGame", 2f);
    }
    void FixedUpdate()
    {
        if (!isGrounded)
        {
            airTime += Time.deltaTime;

            // if (airTime > maxAirTime)
            // {
            //     GameOver();
            // }

            return;
        }
        

        airTime = 0f; // 땅에 닿았으므로 리셋

        // 가속 / 감속
        if (Mathf.Abs(forwardInput) > 0.01f)
        {
            currentSpeed += forwardInput * acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // 방향 전환
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float turnAmount = horizontalInput * turnSpeed * Time.deltaTime;
            Quaternion turnOffset = Quaternion.Euler(0f, turnAmount, 0f);
            transform.rotation *= turnOffset;
        }



        // 지면 기준 이동 방향 계산
        Vector3 slopeForward = Vector3.ProjectOnPlane(transform.forward, lastGroundNormal).normalized; // 방향벡터 변경
        Vector3 move = slopeForward * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;

        // 🔧 바닥 normal 저장
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                lastGroundNormal = contact.normal;
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {

        isGrounded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // 트랙 / 장애물에 부딪쳤을 때
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Treat") || 
            (collision.gameObject.CompareTag("TrackWall") && Mathf.Abs(collision.contacts[0].normal.y) < 0.5f))
        {


            // 속도 리셋 (안튕겨져나오도록)
            currentSpeed = 0f;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            
            // 약간의 회전 효과
            Quaternion softBounce = Quaternion.Euler(0f, 10f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * softBounce, 0.3f);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {       
            UpdateHp(-15);
        }

        if (collision.gameObject.CompareTag("Treat"))
        {
            UpdateHp(-3);
        }

        if (collision.gameObject.CompareTag("UnderGround"))
        {
            // 도로 및 지표면에 떨어졌을 때 게임 오버 처리
            GameOver();
        }

    }


    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateHp(int hpToAdd)
    {
        hp += hpToAdd;
        hpText.text = "hp: " + hp.ToString();

        if (hp <= 0)
        {
            // hp가 0 이하일 때 게임 오버 처리
            GameOver();
        }
    }
}
