using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Zoom Vars

    float zoomSpeed = 4f;
    float minZoom = 1f;
    float maxZoom = 10f;
    float currentZoom = 1f;

    //CameraParams

    Transform mainCamera;
    public Vector3 offsetPos = new Vector3(0, 30, -30);
    public float cameraFollowSpeed = 1.5f;
    public float cameraTurnSpeed = 3f;
    [Range(0.01f, 1f)] public float smothSpeed = 0.1f;
    Vector3 targetPos;
    public float maxMouseVelocity = 4;

    private void Awake()
    {
        mainCamera = Camera.main.transform;
    }

    void Start()
    {
        MoveCamera();
    }

    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        targetPos = transform.position + offsetPos * currentZoom;
        mainCamera.transform.position =
            Vector3.Slerp(mainCamera.transform.position, targetPos, cameraFollowSpeed * smothSpeed);
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        x = Mathf.Clamp(x, -maxMouseVelocity, maxMouseVelocity);
        y = Mathf.Clamp(y, -maxMouseVelocity, maxMouseVelocity);
        offsetPos.y -= y;
        offsetPos.y = Mathf.Clamp(offsetPos.y, 1, 60);
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        mainCamera.transform.LookAt(transform);
        offsetPos = Quaternion.Euler(0, x * cameraTurnSpeed, 0) * offsetPos;

        transform.Rotate(0, x * cameraTurnSpeed, 0); //only for beauty
    }
}