using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class MovementController : MonoBehaviour
{
    public float walkSpeed = 2.5f;
    public float runSpeed = 3.5f;
    public float gravity = -9.8f;

    [Space]
    public float smoothTime = 0.1f;
    private float currentSpeed;
    private float currentVelocitySpeed;

    private float currentVelocityRotate;

    [Space]
    public float jumpForce = 1.35f;
    private float jumpVelocity = 0;

    private CharacterController characterController;

    private Animator animator;
    private new Transform camera;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        camera = Camera.main.transform;
    }

    public void Move(Vector2 _input, bool _isAcceleration)
    {
        if (_input != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg + camera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref currentVelocityRotate, smoothTime);
        }

        float targetSpeed = (_isAcceleration ? runSpeed : walkSpeed) * _input.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref currentVelocitySpeed, smoothTime);

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * gravity + Vector3.up * jumpVelocity;

        characterController.Move(velocity * Time.deltaTime);

        if (jumpVelocity > 0)
            jumpVelocity += gravity * Time.deltaTime;
        else if (jumpVelocity < 0)
            jumpVelocity = 0;

        animator.SetBool("isMove", targetSpeed != 0);
        animator.SetFloat("movementSpeed", targetSpeed * 0.5f);
        animator.SetBool("isGround", characterController.isGrounded);
        if (characterController.isGrounded)
            animator.SetFloat("groundDistance", GetGroundDistance());
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            jumpVelocity = -gravity * jumpForce;
            animator.SetTrigger("jump");
        }
    }

    private float GetGroundDistance()
    {
        float downPoint = transform.position.y + characterController.center.y - characterController.height * 0.5f;
        Vector3 startPoint = transform.position;
        startPoint.y = downPoint + 0.1f;

        if (Physics.Raycast(startPoint, Vector3.down, out RaycastHit hit))
        {
            return (float)System.Math.Round(hit.distance - 0.1f, 2);
        }

        return float.MaxValue;
    }
}
