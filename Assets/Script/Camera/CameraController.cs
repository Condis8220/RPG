using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    [Header("Rotate")]
    public float speedRotate = 10;
    public float minY;
    public float maxY;

    [Header("Zoom")]
    public float speedZoom = 10;
    public float minZoom = 1.5f;
    public float maxZoom = 4f;
    private float distanceFromTarget = 3;

    private float inputX;
    private float inputY;

    private float smoothTime = 0.1f;
    private Vector3 currentRotation;
    private Vector3 currentVelocity;

    private void LateUpdate()
    {
        inputX += Input.GetAxis("Mouse X") * speedRotate;
        inputY -= Input.GetAxis("Mouse Y") * speedRotate;
        inputY = Mathf.Clamp(inputY, minY, maxY);

        distanceFromTarget -= Input.GetAxis("Mouse ScrollWheel") * speedZoom * Time.deltaTime;
        distanceFromTarget = Mathf.Clamp(distanceFromTarget, minZoom, maxZoom);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(inputY, inputX), ref currentVelocity, smoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = (player.position + offset) - transform.forward * distanceFromTarget;
    }
}
