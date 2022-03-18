using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLock : MonoBehaviour
{
    public float mouseSensitivity;

    [SerializeField]GameManager m_gameManager;

    public Transform playerBody;

    private float xRotation = 0f;

    PlayerControls controls;

    private float movementInputX;
    private float movementInputY;

    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();

        controls = new PlayerControls();

        if (m_gameManager.isGamepad)
        {
            controls.Gameplay.Rotation.performed += ctx => movementInputX = ctx.ReadValue<float>();
            controls.Gameplay.Rotation.canceled += ctx => movementInputX = 0f;

            controls.Gameplay.Rotation.performed += ctx => movementInputY = ctx.ReadValue<float>();
            controls.Gameplay.Rotation.canceled += ctx => movementInputY = 0f;
        }
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    public void Update()
    {
        if (m_gameManager.isPc)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
        }
        if (m_gameManager.isGamepad)
        {

            movementInputX = controls.Gameplay.Rotation.ReadValue<float>() * mouseSensitivity * Time.deltaTime;
            movementInputY = controls.Gameplay.Rotation.ReadValue<float>() * mouseSensitivity * Time.deltaTime;

            xRotation -= movementInputY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            //playerBody.transform.localRotation = Quaternion.Euler(0f, xRotation, 0f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.down * movementInputX);

            Debug.Log(movementInputX);
        }

    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }
}
