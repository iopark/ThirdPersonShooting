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

    //BOTH FPS and TPS Camera �� Virtual Camera �� �ƴ� �Ϲ� ī�޶�� �����غ��ҽ��ϴ�. 
    [Header("Pertaining to Camera Movement")]
    [SerializeField] private Vector3 lookDelta; // Mouse input value, determined by changes applied by the pointer movement 
    [SerializeField] private float xRotation; // Camera �� x�����ε� ȸ���� �����ϸ�, (���� movement) 
    [SerializeField] private float yRotation; // ī�޶�� y�����ε� ȸ���� �����ϴ� (�¿� movement) 
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
    /// ī�޶�� ��ǲ���� ���缭 ȸ���մϴ�. 
    /// </summary>
    private void Rotate()
    {
        xRotation -= lookDelta.y * rotationSpeed * Time.deltaTime; // X ������ �����¿�� �����δ�.
                                                                   // Sidenote: ���� �ø����� ��źó�� y ���� �������� ���� �÷�����, ���ϴ� y ���� ������ �������� �÷��̾� �������� �����Ѵ�. 
        yRotation += lookDelta.x * rotationSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0); //�ش� Transform �� ����ȸ���ϴ� ���� Degree���̴�,
    }
}
