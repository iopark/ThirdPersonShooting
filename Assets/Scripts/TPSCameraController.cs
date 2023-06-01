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
        yRotation += lookDelta.x * CameraSensitivity * Time.deltaTime; // y�� ���������� ȸ���� x ���� �������� ȸ�� ��Ų��. 
        xRotation -= lookDelta.y * CameraSensitivity * Time.deltaTime; // x�� ���������� ȸ���� �ԷµǴ� y���� �������� �Է� 
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        // ī�޶� �������� �÷��̾��� �ü��� �������־ TPS Ư���� �°� �����ִ��۾��� �ʿ��ϰڴ�. 
    }
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }
}
