using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLock : MonoBehaviour
{
    private GameManager m_gameManager;

    public float mouseSensitivity;

    public Transform playerBody;
    private float xRotation = 0f;

    PlayerControls controls;

    private Vector2 rotateGamepad;

    private float pitch;

    Quaternion _initRotation;

    [SerializeField] private AssetMenuScriptValue m_assetMenuScriptable;
 
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        
        controls = new PlayerControls();

        _initRotation = transform.localRotation;

        if (m_gameManager.isGamepad)
        {
            controls.Gameplay.Rotation.performed += ctx => rotateGamepad = ctx.ReadValue<Vector2>();
            controls.Gameplay.Rotation.canceled += ctx => rotateGamepad = Vector2.zero;
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

            Vector2 r = new Vector2(rotateGamepad.x, -rotateGamepad.y) * mouseSensitivity * Time.deltaTime;

            transform.localRotation = _initRotation * Quaternion.Euler(pitch, 0f, 0f);

            pitch = Mathf.Clamp(pitch - rotateGamepad.y * 1f, -90f, 90f);

            transform.Rotate(Vector3.right * r.y, Space.Self);

            playerBody.Rotate(Vector3.up * r.x, Space.Self);

        }
        mouseSensitivity = m_assetMenuScriptable.value;
    }
    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }
}