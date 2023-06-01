using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HW_PlayerMover : MonoBehaviour
{
    [Header("Required Components")]
    private CharacterController characterController;

    [Header("Pertaining to Movement")]
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float yVelocity;

    [SerializeField] private float gunPointDirection;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        moveSpeed = 10f;
        jumpSpeed = 5f; 
    }

    private void Update()
    {
        Move();
        Jump(); 
    }

    private void Move()
    {
        Vector3 focusPoint = Quaternion.Euler(0f, gunPointDirection, 0f) * transform.forward; // 총구시점의 각도에 플레이어의 z값을 곱한값 
        // where UnitVector * a angle would shift that unit vector in accordance to a given *new* angle value. 

        // 그럼 플레이어의 앞뒤 움직임은 이제 총구방향을 움직이게 된다. 
        
        // 플레이어의 회전값 계산 
        // using Mathf.tan2, which takes in Opp, Hyp value, given the player's forward position in 2D grid is pi/2, 
        // because we already know the target direction (focusPoint), take x, y of that, multiply by the 
        characterController.Move(focusPoint * moveDir.z * moveSpeed * Time.deltaTime);
        characterController.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
        
    }

    private void Jump()
    {
        // using method in the lecture, using game's preset gravity value; 
        yVelocity += Physics.gravity.y * Time.deltaTime; // falling speed, constantly changing by constant acceleration speed of the gravity, by default, -9.81N 
        if (CheckGround() && yVelocity < 0)
            yVelocity = 0;
        if (yVelocity <= Physics.gravity.y) // to make sure falling speed reaches an equilibrium point based on the player's mass in relation to its gravity 
            yVelocity = Physics.gravity.y;

        characterController.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDir = new Vector3(input.x, 0, input.y); 
    }

    private void OnJump(InputValue value)
    {
        if (CheckGround())
            yVelocity = jumpSpeed; // directly apply object's y-axis movement velocity with the fixed value; 
    }

    private bool CheckGround()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.7f); 
    }
}
