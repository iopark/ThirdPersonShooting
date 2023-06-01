using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;
    [SerializeField] Transform cameraRoot; 

    [Header("Relating to the rotation")]
    private Vector2 lookDelta;
    private float xRotation; 
    private float yRotation;

    private void Awake()
    {
        //cameraRoot = GetComponent<Transform>(); 
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 중앙에 고정 및 보이지 않음 
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None; // 마우스 커서 다시 비해제, 다시 보임. 
    }

    private void LateUpdate()
    {
        Look(); 
    }

    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime; // y축 기준으로의 회전은 x 값을 기준으로 회전 시킨다. 
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime; // x축 기준으로의 회전은 입력되는 y값을 기준으로 입력 
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraRoot.localRotation = Quaternion.Euler(xRotation,0,0);
        transform.localRotation = Quaternion.Euler(0,yRotation,0); 
    }

    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }
}
