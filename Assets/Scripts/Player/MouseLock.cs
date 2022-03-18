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

    private Vector2 rotateGamepad;
    private float movementInputY;
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();

        controls = new PlayerControls();

        if (m_gameManager.isGamepad)
        {
            controls.Gameplay.Rotation.performed += ctx => rotateGamepad = ctx.ReadValue<Vector2>();
            controls.Gameplay.Rotation.canceled += ctx => rotateGamepad = Vector2.zero;

            //controls.Gameplay.Rotation.performed += ctx => movementInputY = ctx.ReadValue<Vector2>();
            //controls.Gameplay.Rotation.canceled += ctx => movementInputY = 0f;

            Debug.Log("valeures lues");
        }
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    public void Update()
    {

        if (m_gameManager.isPc == true)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
        }
        else if (m_gameManager.isGamepad == true)
        {

            //rotateGamepad = controls.Gameplay.Rotation.ReadValue<float>() * mouseSensitivity * Time.deltaTime;

            //xRotation -= movementInputY;

            //transform.localRotation = Quaternion.Euler(0f, xRotation, 0f);

            //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            //playerBody.Rotate(Vector3.down * movementInputX);

            Debug.Log(rotateGamepad);

            Vector2 r = new Vector2(-rotateGamepad.x, -rotateGamepad.y) * 100f * Time.deltaTime;
            transform.localRotation = Quaternion.Euler(rotateGamepad.y, 0f, 0f);
            playerBody.Rotate(r, Space.Self);
            Debug.Log(r);
            
        }

    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }
}
