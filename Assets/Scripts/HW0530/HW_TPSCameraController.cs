using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HW_TPSCameraController : MonoBehaviour
{
    // �÷��̾�� ī�޶��� �߽��� �������� �Ӹ��� �������߸� �Ѵ�. 
    [Header("CameraView")]
    private GameObject player;
    public Transform shoulder;
    private Camera self;
    private Vector3 gunPoint; 

    //BOTH FPS and TPS Camera �� Virtual Camera �� �ƴ� �Ϲ� ī�޶�� �����غ��ҽ��ϴ�. 
    [Header("Pertaining to Camera Movement")]
    [SerializeField] private Vector3 lookDelta; // Mouse input value, determined by changes applied by the pointer movement 
    [SerializeField] private float xRotation; // Camera �� x�����ε� ȸ���� �����ϸ�, (���� movement) 
    [SerializeField] private float yRotation; // ī�޶�� y�����ε� ȸ���� �����ϴ� (�¿� movement) 
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
        xRotation -= lookDelta.y * rotationSpeed * Time.deltaTime; // X ������ �����¿�� �����δ�.                                                 // Sidenote: ���� �ø����� ��źó�� y ���� �������� ���� �÷�����, ���ϴ� y ���� ������ �������� �÷��̾� �������� �����Ѵ�. 
        yRotation += lookDelta.x * rotationSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); //�ش� Transform �� ����ȸ���ϴ� ���� Degree���̴�,
    }

    /// <summary>
    /// ī�޶�� ��ǲ���� ���缭 ȸ���մϴ�. 
    /// </summary>
    private void Rotate()
    {
        gunPoint = self.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1));
        gunPoint.y = player.transform.position.y;
        HeadBob();
    }
}
