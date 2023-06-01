using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HW_TPSCameraController : MonoBehaviour
{
    // Taking approach from https://www.youtube.com/watch?v=owW7BE2t8ME&t=770s by Deniz SimSek. 
    // And Brackeys, https://www.youtube.com/watch?v=4HpC--2iowE 

    /* The main focus here is to seperate 
     * 1. Orbiting camera and its rotation 
     * 2. given a 'focal point' in which player should rotate to, simply do so in the playercontroller page
     */ 
    // 플레이어는 카메라의 중심을 기점으로 머리를 움직여야만 한다. 
    [Header("CameraView")]
    private GameObject player; // 회전축의 중심이기도 하다. 
    public Transform shoulder;
    private Camera self;
    [SerializeField] private Vector3 gunPoint; 

    //BOTH FPS and TPS Camera 는 Virtual Camera 가 아닌 일반 카메라로 구현해보았습니다. 
    [Header("Pertaining to Camera Movement")]
    [SerializeField] private Vector3 lookDelta; // Mouse input value, determined by changes applied by the pointer movement 
    [SerializeField] private float xRotation; // Camera 는 x축으로도 회전이 가능하며, (상하 movement) 
    [SerializeField] private float yRotation; // 카메라는 y축으로도 회전이 가능하다 (좌우 movement) 
    [SerializeField] private Transform playerRotation; 
    [Range(0f, 5f)]
    private float rotationSpeed;
    [SerializeField] private Vector3 offSet; // Radius 이기도 한 값이며, 

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        transform.position = shoulder.transform.position;
        transform.forward = player.transform.forward;
        transform.rotation = shoulder.transform.rotation;
        offSet = shoulder.transform.position - player.transform.position;
        //transform.SetParent(shoulder.transform); 
        self = GetComponent<Camera>();
    }
    private void Start()
    {
        //offSet = shoulder.position - player.transform.position;
        gunPoint = self.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1)); // 뷰프러스텀 기준 가장 최종 사각형의 중앙값을 기준으로 플레이어는 집중한다. (in some cases, fps게임에서는 z값을 0으로 잡고 조준점으로 잡는경우도 있다) 
        rotationSpeed = 3f;
    }
    private void Update()
    {
        Look();
    }
    private void LateUpdate()
    {
        //transform.position = player.transform.position + offSet; 
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

    private void CameraOrbit()
    {
        //float x = player.transform.position.x + offSet.magnitude * Mathf.Cos(playerRotation * Mathf.Deg2Rad);
        //float z = player.transform.position.z + offSet.magnitude * Mathf.Sin(playerRotation * Mathf.Deg2Rad);
        //Vector3 newPosition = new Vector3(x, shoulder.transform.position.y, z);
        //// Move the object to the desired position
        //transform.forward = shoulder.transform.forward;
        //transform.position = newPosition;
        transform.RotateAround(player.transform.position, Vector3.up, yRotation); 
        
    }

    private void HeadBob()
    {
        //Vector3 direction = gunPoint - player.transform.position;// 플레이어와 카메라 정중앙 위치의 벡터를 구한뒤 
        //direction.y = player.transform.position.y; // y 값은 플레이어의 값과 동일하게 하여 x축에 대한 회전은 그냥 방지한다 // 이럴땐 Bobing해드를 만들어 머리만 댕강움직이게 한다. 
        //Quaternion rotation = Quaternion.FromToRotation(player.transform.forward, direction);
        //// Apply the rotation to the GameObject, keeping its original rotation on other axes
        //rotation.y = player.transform.position.y;
        //playerRotation = rotation.eulerAngles.y;
        //player.transform.rotation *= rotation; 

        //Vector3 direction = gunPoint - player.transform.position;
        //direction.y = player.transform.position.y;
        ////direction.z = player.transform.position.z;
        //Quaternion rotation = Quaternion.FromToRotation(player.transform.position, direction);
        ////Quaternion forwardRotation = Quaternion.AngleAxis(player.transform.rotation.eulerAngles.y, Vector3.up);
        ////Quaternion finalRotation = rotation * forwardRotation;
        //playerRotation = rotation.eulerAngles.y;
        //player.transform.rotation = rotation; 
        Vector3 direction = gunPoint - player.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, player.transform.up);
        rotation.y = 0;
        player.transform.rotation = rotation; 
    }

    private void Look()
    {
        xRotation -= lookDelta.y * rotationSpeed * Time.deltaTime; // X 축으로 상하좌우로 움직인다.                                                 // Sidenote: 위로 올릴때는 포탄처럼 y 값이 내려갈때 위로 올려보니, 변하는 y 값을 내려서 보편적인 플레이어 움직임을 구현한다. 
        yRotation += lookDelta.x * rotationSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); //해당 Transform 의 로컬회전하는 값은 Degree값이다,
        //playerRotation.rotation = transform.rotation; 
        gunPoint = self.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1));
        CameraOrbit();
    }

    /// <summary>
    /// 카메라는 인풋값에 맞춰서 회전합니다. 
    /// </summary>
    private void Rotate()
    {
        HeadBob();

    }
}
