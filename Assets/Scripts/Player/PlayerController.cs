using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    //Si le joueur ne peut pas monter les escalier il faut changer le step offset dans unity ou dans le code du chara controller

    [SerializeField]private CharacterController m_myChara;

    [SerializeField]GameManager m_gameManager;

    [SerializeField] UiManager uimanager;

    [SerializeField, Tooltip("Vitesse du joueur")]private float m_speed;

    private float m_gravity = -9.81f;

    Vector3 m_velocity;

    [SerializeField, Tooltip("Transform d'un empty ou sera crée la sphere pour savoir si le joueur est sur le sol")]private Transform groundCheck;

    [SerializeField, Tooltip("Radius de la sphere qui check si le joueur est sur le sol")]private float radiusCheckSphere = 0.4f;

    [SerializeField, Tooltip("Mask ou l'on définit le sol")]private LayerMask m_groundMask;

    private bool m_isGrounded;      //Si le joueur est sur le sol ?

    PlayerControls controls;
    Vector2 GamepadMove;


    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        controls = new PlayerControls();

        if (m_gameManager.isGamepad)
        {
            controls.Gameplay.GamepadMove.performed += ctx => GamepadMove = ctx.ReadValue<Vector2>();
            controls.Gameplay.GamepadMove.canceled += ctx => GamepadMove = Vector2.zero;
            Debug.Log("valeures lues");
        }
    }
    public void Update()
    {

        if (m_gameManager.isPc)
        {
            m_isGrounded = Physics.CheckSphere(groundCheck.position, radiusCheckSphere, m_groundMask);      //Création d'une sphere qui chech si le joueur touche le sol

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

            //Activation de la lampe
            ActiveFlashlight();
        }
        else if (m_gameManager.isGamepad)
        {
            Vector2 m = new Vector2(-GamepadMove.x, GamepadMove.y) * Time.deltaTime;
            transform.Translate(m, Space.World);
            Debug.Log(m);
        }
       
    }

    //Variables, références et fonctions de la lampe par rapport au joueur
    [SerializeField]FlashlightManager flm;
    public bool flashlightIsPossessed = false;
    public void takeFlashlight()
    {
        if (m_gameManager.isPc)
        {
            if (Input.GetKey(KeyCode.E))
            {
                uimanager.DisableUi();
                flm.PickItem();
                flashlightIsPossessed = true;
            }
        }
        else if (m_gameManager.isGamepad)
        {
            uimanager.DisableUi();
            flm.PickItem();
            flashlightIsPossessed = true;
        }
        
    }
    public void ActiveFlashlight()
    {
        if (m_gameManager.isPc)
        {
            if (Input.GetKeyDown(KeyCode.F) && flashlightIsPossessed == true)
            {
                flm.UseFlashlight();
            }
            if (Input.GetKey(KeyCode.G) && flashlightIsPossessed == true)
            {
                flm.DropItem();
                flashlightIsPossessed = false;
            }
        }
       

    }

}
