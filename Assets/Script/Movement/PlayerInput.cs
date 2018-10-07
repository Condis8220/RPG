using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class PlayerInput : MonoBehaviour
{
    public KeyCode moveForward = KeyCode.W;
    public KeyCode moveBack = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;

    [Space]
    public KeyCode acceleration = KeyCode.LeftShift;

    [Space]
    public KeyCode jump = KeyCode.Space;

    private MovementController movementController;

    private void Start()
    {
        movementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        Vector2 input = Vector2.zero;

        if (Input.GetKey(moveForward))
            input.y = 1;
        else if (Input.GetKey(moveBack))
            input.y = -1;

        if (Input.GetKey(moveLeft))
            input.x = -1;
        else if (Input.GetKey(moveRight))
            input.x = 1;

        if (Input.GetKeyDown(jump))
            movementController.Jump();

        movementController.Move(input, Input.GetKey(acceleration));
    }
}
