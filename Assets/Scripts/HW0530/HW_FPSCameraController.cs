using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class HW_FPSCameraController : MonoBehaviour
{
    [Header("CameraView")]
    private GameObject player;
    private Camera cam; 

    //BOTH FPS and TPS Camera 는 Virtual Camera 가 아닌 일반 카메라로 구현해보았습니다. 
    [Header("Pertaining to Camera Movement")]
    [SerializeField] private Vector3 lookDelta; // Mouse input value, determined by changes applied by the pointer movement 
    [SerializeField] private float xRotation; // Camera 는 x축으로도 회전이 가능하며, (상하 movement) 
    [SerializeField] private float yRotation; // 카메라는 y축으로도 회전이 가능하다 (좌우 movement) 
    [Range(0f, 5f)]
    private float rotationSpeed;

    private void Awake()
    {
        cam = gameObject.GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        transform.SetParent(player.transform);
        Vector3 setCameraPos = player.transform.position;
        setCameraPos.y = 1.63f;
        // Given player's head's height = 1.63f; 
        transform.position = setCameraPos; 

            //new Vector3(player.transform.position.x, 1.63f, player.transform.position.y);
        // 1. set camera's forward as player's forward 
        transform.rotation = player.transform.rotation;
    }
    private void Start()
    {
        rotationSpeed = 3f; 
    }

    private void LateUpdate()
    {
        Rotate(); 
    }
    private void OnLook(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        lookDelta = new Vector3(input.x, input.y, 0); 
    }

    private void SetPosition()
    {
        Vector3 aimpoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
    }

    /// <summary>
    /// 카메라는 인풋값에 맞춰서 회전합니다. 
    /// </summary>
    private void Rotate()
    {
        xRotation -= lookDelta.y * rotationSpeed * Time.deltaTime; // X 축으로 상하좌우로 움직인다.
                                                                   // Sidenote: 위로 올릴때는 포탄처럼 y 값이 내려갈때 위로 올려보니, 변하는 y 값을 내려서 보편적인 플레이어 움직임을 구현한다. 
        yRotation += lookDelta.x * rotationSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0); //해당 Transform 의 로컬회전하는 값은 Degree값이다,
    }
}
