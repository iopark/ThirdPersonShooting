using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HW_TPSCameraController : MonoBehaviour
{
    // 플레이어는 카메라의 중심을 기점으로 머리를 움직여야만 한다. 
    [Header("CameraView")]
    private GameObject player;
    public Transform shoulder;
    private Camera self;
    private Vector3 gunPoint; 

    //BOTH FPS and TPS Camera 는 Virtual Camera 가 아닌 일반 카메라로 구현해보았습니다. 
    [Header("Pertaining to Camera Movement")]
    [SerializeField] private Vector3 lookDelta; // Mouse input value, determined by changes applied by the pointer movement 
    [SerializeField] private float xRotation; // Camera 는 x축으로도 회전이 가능하며, (상하 movement) 
    [SerializeField] private float yRotation; // 카메라는 y축으로도 회전이 가능하다 (좌우 movement) 
    [Range(0f, 5f)]
    private float rotationSpeed;
    [SerializeField] private Vector3 offSet; 

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        transform.position = shoulder.transform.position;
        transform.forward = shoulder.transform.forward;
        transform.rotation = shoulder.transform.rotation;
        //transform.SetParent(shoulder.transform); 
        self = GetComponent<Camera>();
    }
    private void Start()
    {
        //offSet = shoulder.position - player.transform.position;
        gunPoint = self.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1));
        rotationSpeed = 3f;
    }
    private void Update()
    {
        
    }
    private void LateUpdate()
    {
        Look();
        Rotate();
    }
    private void OnLook(InputValue value)
    {
        
        Vector2 input = value.Get<Vector2>();
        lookDelta = new Vector3(input.x, input.y, 0);
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    private void HeadBob()
    {
        player.transform.LookAt(gunPoint); 
    }

    private void Look()
    {
        xRotation -= lookDelta.y * rotationSpeed * Time.deltaTime; // X 축으로 상하좌우로 움직인다.                                                 // Sidenote: 위로 올릴때는 포탄처럼 y 값이 내려갈때 위로 올려보니, 변하는 y 값을 내려서 보편적인 플레이어 움직임을 구현한다. 
        yRotation += lookDelta.x * rotationSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); //해당 Transform 의 로컬회전하는 값은 Degree값이다,
    }

    /// <summary>
    /// 카메라는 인풋값에 맞춰서 회전합니다. 
    /// </summary>
    private void Rotate()
    {
        gunPoint = self.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1));
        gunPoint.y = player.transform.position.y;
        HeadBob();
    }
}
