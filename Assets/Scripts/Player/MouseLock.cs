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
    private Vector2 rotate;

    private void Awake()
    {
        isGamepad = true;
        controls = new PlayerControls();

        if (isGamepad)
        {
            controls.Gameplay.Rotation.performed += ctx => rotate = ctx.ReadValue<Vector2>();
            controls.Gameplay.Rotation.canceled += ctx => rotate = Vector2.zero;
            Debug.Log("les valeures sont lues");
        }
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
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
            //NE MARCHE PAS ENCORE
            Vector2 look = new Vector2(-rotate.y, -rotate.x) * mouseSensitivity * Time.deltaTime;
            playerBody.Rotate(look, Space.World);
            Debug.Log(look);
        }
        
    }
}
