using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    //Si le joueur ne peut pas monter les escalier il faut changer le step offset dans unity ou dans le code du chara controller

    [SerializeField, Tooltip("Références du Chara controller")]private CharacterController m_myChara;

    private GameManager m_gameManager;

    [SerializeField, Tooltip("Références de l'uiManager")]private UiManager m_uiManager;

    [SerializeField, Tooltip("Vitesse du joueur en m/s")]private float m_speed;

    [SerializeField, Tooltip("Gravité du joueur en m/s")]private float m_gravity = -9.81f;

    private Vector3 m_velocity;
    
    [SerializeField, Tooltip("Transform d'un empty ou sera crée la sphere pour savoir si le joueur est sur le sol")]private Transform m_groundCheck;

    [SerializeField, Tooltip("Radius de la sphere qui check si le joueur est sur le sol")]private float m_radiusSphere = 0.4f;

    [SerializeField, Tooltip("Mask ou l'on définit le sol")]private LayerMask m_groundMask;

    private bool m_isGrounded;      //Si le joueur est sur le sol ?

    private PlayerControls m_controls;

    private Vector2 playerMove;


    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_controls = new PlayerControls();
    }
    public void Update()
    {
        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_radiusSphere, m_groundMask);      //Création d'une sphere qui chech si le joueur touche le sol

        if (m_isGrounded && m_velocity.y < 0)        //Reset de la gravité quand le joueur touche le sol
        {
            m_velocity.y = -2f;
        }

        // Déplacements du joueur
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        m_myChara.Move(move * m_speed * Time.deltaTime);

        m_myChara.Move(m_velocity * Time.deltaTime);

        m_velocity.y += m_gravity * Time.deltaTime;

        //Drop de la lampe
        DropFlashlight();

        if (m_gameManager.isPc)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_uiManager.MenuPause();
            }
        }
        else if (m_gameManager.isGamepad)
        {
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                m_uiManager.MenuPause();
            }
        }
        
    }

    //Variables, références et fonctions de la lampe par rapport au joueur
    [SerializeField]FlashlightManager flm;
    public bool m_flashlightIsPossessed = false;
    public void TakeFlashlight()
    {
        if (m_gameManager.isPc == true)
        {
            if (Input.GetKey(KeyCode.E))
            {
                m_uiManager.DisableUi();
                flm.PickItem();
                m_flashlightIsPossessed = true;
            }
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.buttonEast.isPressed)
            {
                m_uiManager.DisableUi();
                flm.PickItem();
                Gamepad.current.SetMotorSpeeds(0.75f * Time.deltaTime, 0.75f * Time.deltaTime);
                float frequency = InputSystem.pollingFrequency = 60f;
                m_flashlightIsPossessed = true;
            }
        } 
    }
    public void DropFlashlight()
    {
        if (m_gameManager.isPc == true)
        {
            if (Input.GetKey(KeyCode.G) && m_flashlightIsPossessed == true)
            {
                flm.DropItem();
                m_flashlightIsPossessed = false;
            }
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.buttonWest.isPressed && m_flashlightIsPossessed == true)
            {
                flm.DropItem();
                m_flashlightIsPossessed = false;
            }
        }
    }

}
