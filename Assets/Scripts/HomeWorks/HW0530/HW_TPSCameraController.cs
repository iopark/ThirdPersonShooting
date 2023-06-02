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
    // �÷��̾�� ī�޶��� �߽��� �������� �Ӹ��� �������߸� �Ѵ�. 
    [Header("CameraView")]
    private GameObject player; // ȸ������ �߽��̱⵵ �ϴ�. 
    public Transform shoulder;
    private Camera self;
    [SerializeField] private Vector3 gunPoint; 

    //BOTH FPS and TPS Camera �� Virtual Camera �� �ƴ� �Ϲ� ī�޶�� �����غ��ҽ��ϴ�. 
    [Header("Pertaining to Camera Movement")]
    [SerializeField] private Vector3 lookDelta; // Mouse input value, determined by changes applied by the pointer movement 
    [SerializeField] private float xRotation; // Camera �� x�����ε� ȸ���� �����ϸ�, (���� movement) 
    [SerializeField] private float yRotation; // ī�޶�� y�����ε� ȸ���� �����ϴ� (�¿� movement) 
    [SerializeField] private Transform playerRotation; 
    [Range(0f, 5f)]
    private float rotationSpeed;
    [SerializeField] private Vector3 offSet; // Radius �̱⵵ �� ���̸�, 

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
        gunPoint = self.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1)); // ���������� ���� ���� ���� �簢���� �߾Ӱ��� �������� �÷��̾�� �����Ѵ�. (in some cases, fps���ӿ����� z���� 0���� ��� ���������� ��°�쵵 �ִ�) 
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
        //Vector3 direction = gunPoint - player.transform.position;// �÷��̾�� ī�޶� ���߾� ��ġ�� ���͸� ���ѵ� 
        //direction.y = player.transform.position.y; // y ���� �÷��̾��� ���� �����ϰ� �Ͽ� x�࿡ ���� ȸ���� �׳� �����Ѵ� // �̷��� Bobing�ص带 ����� �Ӹ��� �󰭿����̰� �Ѵ�. 
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
        xRotation -= lookDelta.y * rotationSpeed * Time.deltaTime; // X ������ �����¿�� �����δ�.                                                 // Sidenote: ���� �ø����� ��źó�� y ���� �������� ���� �÷�����, ���ϴ� y ���� ������ �������� �÷��̾� �������� �����Ѵ�. 
        yRotation += lookDelta.x * rotationSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); //�ش� Transform �� ����ȸ���ϴ� ���� Degree���̴�,
        //playerRotation.rotation = transform.rotation; 
        gunPoint = self.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1));
        CameraOrbit();
    }

    /// <summary>
    /// ī�޶�� ��ǲ���� ���缭 ȸ���մϴ�. 
    /// </summary>
    private void Rotate()
    {
        HeadBob();

    }
}
