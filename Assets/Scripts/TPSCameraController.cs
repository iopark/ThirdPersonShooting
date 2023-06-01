using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] Transform cameraRoot;
    [SerializeField] Transform aimTarget;
    //[SerializeField] Camera camera;

    [SerializeField] float CameraSensitivity;
    [SerializeField] float lookDistance;

    private Vector2 lookDelta; 
    private float xRotation; 
    private float yRotation;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None; 
    }
    private void Update()
    {
        Rotate(); 
    }
    private void Rotate()
    {
        Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;
        aimTarget.position = lookPoint;
        lookPoint.y = transform.position.y; // where player y-axis = 0; 
        //Vector3 cameraCenter = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)); 
        transform.LookAt(lookPoint); // player will always based on the camera 
    }
    private void LateUpdate()
    {
        Look(); 
    }
    private void Look()
    {
        yRotation += lookDelta.x * CameraSensitivity * Time.deltaTime; // y축 기준으로의 회전은 x 값을 기준으로 회전 시킨다. 
        xRotation -= lookDelta.y * CameraSensitivity * Time.deltaTime; // x축 기준으로의 회전은 입력되는 y값을 기준으로 입력 
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        // 카메라를 기준으로 플레이어의 시선을 설정해주어서 TPS 특성에 맞게 맞춰주는작업이 필요하겠다. 
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }
}
