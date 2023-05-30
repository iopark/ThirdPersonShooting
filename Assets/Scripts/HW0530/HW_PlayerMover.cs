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
        characterController.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
        characterController.Move(transform.right * moveDir.x * moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        // using method in the lecture, using game's preset gravity value; 
        yVelocity += Physics.gravity.y * Time.deltaTime; // falling speed, constantly changing by constant acceleration speed of the gravity, by default, -9.81N 
        if (CheckGround() && yVelocity < 0)
            yVelocity = 0;
        if (yVelocity >= Physics.gravity.y) // to make sure falling speed reaches an equilibrium point based on the player's mass in relation to its gravity 
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
