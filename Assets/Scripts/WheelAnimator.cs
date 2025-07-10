using UnityEngine;

public class WheelAnimator : MonoBehaviour
{
    [Header("Front Wheels")]
    public Transform wheelFL;
    public Transform wheelFR;

    [Header("Back Wheels")]
    public Transform wheelBL;
    public Transform wheelBR;

    [Header("Steering Settings")]
    public float maxSteeringAngle = 30f;
    public float steeringSpeed = 90f;

    [Header("Rotation Settings")]
    public float wheelRotationSpeed = 120f; 

    private float currentSteeringAngle = 0f;
    private float wheelSpinAngle = 0f;
    private float forwardInput;

    // 초기 회전 / 위치 저장
    private Quaternion initRotFL, initRotFR, initRotBL, initRotBR;
    private Vector3 initPosFL, initPosFR, initPosBL, initPosBR;

    void Start()
    {
        // 초기 위치 및 회전값 저장
        initRotFL = wheelFL.localRotation;
        initRotFR = wheelFR.localRotation;
        initRotBL = wheelBL.localRotation;
        initRotBR = wheelBR.localRotation;

        initPosFL = wheelFL.localPosition;
        initPosFR = wheelFR.localPosition;
        initPosBL = wheelBL.localPosition;
        initPosBR = wheelBR.localPosition;
    }

    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        HandleSteering();
        HandleRotation();
    }

    void HandleSteering()
    {
        float targetAngle = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            targetAngle = -maxSteeringAngle;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            targetAngle = maxSteeringAngle;
        }

        currentSteeringAngle = Mathf.MoveTowards(
            currentSteeringAngle,
            targetAngle,
            steeringSpeed * Time.deltaTime
        );

        ApplyRotation(); // ← steeringAngle 변화가 생기면 바로 반영
    }

    void HandleRotation()
    {
        float rotationThisFrame = forwardInput * wheelRotationSpeed * Time.deltaTime;

        if (Mathf.Abs(rotationThisFrame) >= 0.01f)
        {
            wheelSpinAngle += rotationThisFrame;
            ApplyRotation(); // ← 바퀴가 굴러도 적용
        }
    }

    void ApplyRotation()
    {

        Quaternion spinRot = Quaternion.Euler(0f, -wheelSpinAngle, 0f);
        Quaternion steerRot = Quaternion.Euler(currentSteeringAngle, 0f, 0f);
    
        // 앞바퀴: steering + rolling
        wheelFL.localRotation = initRotFL * steerRot * spinRot;
        wheelFR.localRotation = initRotFR * steerRot * spinRot;
    
        // 뒷바퀴: rolling만
        wheelBL.localRotation = initRotBL * spinRot;
        wheelBR.localRotation = initRotBR * spinRot;
    
        // 위치 고정
        wheelFL.localPosition = initPosFL;
        wheelFR.localPosition = initPosFR;
        wheelBL.localPosition = initPosBL;
        wheelBR.localPosition = initPosBR;
    }
}
