using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLock : MonoBehaviour
{
    private GameManager m_gameManager;

    [Tooltip("R�f�rence du transform du joueur")]public Transform playerBody;
    private float xRotation = 0f;

    PlayerControls controls;

    private Vector2 rotateGamepad;

    float pitch;
    public float mouseSensitivity;
    Quaternion _initRotation;

    [SerializeField] private AssetMenuScriptValue m_assetMenuScriptable;
    [SerializeField] private AudioManagerScript m_audioScript;

<<<<<<< HEAD
    private bool m_isActivated;

    private void Awake()
=======
    private void Start()
>>>>>>> origin/dev_Thomas
    {
        m_isActivated = true;
        
        m_gameManager = FindObjectOfType<GameManager>();

        controls = new PlayerControls();

        _initRotation = transform.localRotation;
            
        if (m_gameManager.isGamepad)
        {
            controls.Gameplay.Rotation.performed += ctx => rotateGamepad = ctx.ReadValue<Vector2>();
            controls.Gameplay.Rotation.canceled += ctx => rotateGamepad = Vector2.zero;
        }
        m_audioScript.PlayMusic("AmbiantMusic");
    }

    // Update is called once per frame
    public void Update()
    {
        Debug.Log($"heu,c'est le state du lock :{m_isActivated}");
        if (m_gameManager.isPc)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
<<<<<<< HEAD
            
            if (m_isActivated)
            {
                playerBody.Rotate(Vector3.up * mouseX);
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
=======

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
            Debug.Log("gameManager true");
>>>>>>> origin/dev_Thomas
        }
        else if (m_gameManager.isGamepad)
        {

            Vector2 r = new Vector2(rotateGamepad.x, -rotateGamepad.y) * mouseSensitivity * Time.deltaTime;
            
            if (m_isActivated)
            {
                transform.localRotation = _initRotation * Quaternion.Euler(pitch, 0f, 0f);

                pitch = Mathf.Clamp(pitch - rotateGamepad.y * 1f, -90f, 90f);
                
                transform.Rotate(Vector3.right * r.y, Space.Self);

                playerBody.Rotate(Vector3.up * r.x, Space.Self);
            }
        }
        mouseSensitivity = m_assetMenuScriptable.value;
    }

    private void OnEnable()
    {
        
    }

    public void IsMoving(bool p_activated)
    {
        m_isActivated = p_activated;
    }
}