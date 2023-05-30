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
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ�� �߾ӿ� ���� �� ������ ���� 
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None; // ���콺 Ŀ�� �ٽ� ������, �ٽ� ����. 
    }

    private void LateUpdate()
    {
        Look(); 
    }

    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime; // y�� ���������� ȸ���� x ���� �������� ȸ�� ��Ų��. 
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime; // x�� ���������� ȸ���� �ԷµǴ� y���� �������� �Է� 
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); 

        cameraRoot.localRotation = Quaternion.Euler(xRotation,0,0);
        transform.localRotation = Quaternion.Euler(0,yRotation,0); 
    }

    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }
}
