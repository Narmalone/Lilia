using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLock : MonoBehaviour
{
    public float mouseSensitivity;

    public Transform playerBody;

    private float xRotation = 0f;

    public bool isGamepad = true;
    public bool isPc = true;
    PlayerControls controls;

    private float movementInputX;
    private float movementInputY;

    private void Awake()
    {
        isGamepad = true;
        controls = new PlayerControls();

        if (isGamepad)
        {
            controls.Gameplay.Rotation.performed += ctx => movementInputX = ctx.ReadValue<float>();
            controls.Gameplay.Rotation.canceled += ctx => movementInputX = 0f;

            controls.Gameplay.Rotation.performed += ctx => movementInputY = ctx.ReadValue<float>();
            controls.Gameplay.Rotation.canceled += ctx => movementInputY = 0f;
            Debug.Log("les valeures sont lues");
        }
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    public void Update()
    {
        if (isPc)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
        }
        if (isGamepad)
        {

            movementInputX = controls.Gameplay.Rotation.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
            movementInputY = controls.Gameplay.Rotation.ReadValue<float>() * mouseSensitivity * Time.deltaTime;

            xRotation -= movementInputY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(0f, xRotation, 0f);
            playerBody.Rotate(Vector3.down * movementInputX);
            Debug.Log(movementInputX);
        }

    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }
}
