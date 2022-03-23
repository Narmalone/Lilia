using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using Unity.VisualScripting;
using System.Threading.Tasks;
public class PlayerController : MonoBehaviour
{
    //----------------------------------------------- References from other Class ------------------------------------------//

    private GameManager m_gameManager;

    [SerializeField, Tooltip("Références de l'uiManager")]private UiManager m_uiManager;

    [SerializeField] FlashlightManager m_flm;
    DoudouManager m_doudouManager;

    [SerializeField] private LayerMask m_flashlightMask;
    [SerializeField] private LayerMask m_doudouMask;

    //----------------------------------------------- Player controls system ------------------------------------------//


    [SerializeField, Tooltip("Références du Chara controller")] private CharacterController m_myChara;


    [SerializeField, Tooltip("Vitesse du joueur en m/s")]private float m_speed;

    [SerializeField, Tooltip("Gravité du joueur en m/s")]private float m_gravity = -9.81f;

    private Vector3 m_velocity;
    
    [SerializeField, Tooltip("Transform d'un empty ou sera crée la sphere pour savoir si le joueur est sur le sol")]private Transform m_groundCheck;

    [SerializeField, Tooltip("Radius de la sphere qui check si le joueur est sur le sol")]private float m_radiusSphere = 0.4f;

    [SerializeField, Tooltip("Mask ou l'on définit le sol")]private LayerMask m_groundMask;

    private bool m_isGrounded;      //Si le joueur est sur le sol ?

    private PlayerControls m_controls;

    private Vector2 playerMove;

    //----------------------------------------------- Player Items Systems ------------------------------------------//

    public bool m_flashlightIsPossessed = false;
    public bool m_doudouIsPossessed = false;
    public bool isFullItem = false;
    //----------------------------------------------- Post-processing ------------------------------------------//


    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_controls = new PlayerControls();
        m_doudouManager = FindObjectOfType<DoudouManager>();
        m_flm = FindObjectOfType<FlashlightManager>();
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


        //Check si le joueur drop des items
        DropFlashlight();
        DropDoudou();
    }

    private void OnTriggerStay(Collider p_collide)
    {
        if (m_gameManager.isPc)
        {

            if ((m_flashlightMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {
                Debug.Log("dans la condition bitch");

                if (m_flashlightIsPossessed == false)
                {
                    TakeFlashlight();
                    m_uiManager.TakeObject();
                }
                else if (m_flashlightIsPossessed == true)
                {
                    m_uiManager.DisableUi();
                }
            }

            else if ((m_doudouMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {
                Debug.Log("dans la condition bitch");

                if (m_doudouIsPossessed == false)
                {
                    TakeDoudou();
                    m_uiManager.TakeObject();
                }
                else if (m_doudouIsPossessed == true)
                {
                    m_uiManager.DisableUi();
                }
            }
        }
        else if (m_gameManager.isGamepad)
        {
            if ((m_flashlightMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {

                if (m_flashlightIsPossessed == false)
                {
                    TakeFlashlight();
                    m_uiManager.TakeObject();
                    Debug.Log("gamepad ui activée");
                }
                else if (m_flashlightIsPossessed == true)
                {
                    m_uiManager.DisableUi();
                }
            }
            else if ((m_doudouMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {

                if (m_doudouIsPossessed == false)
                {
                    TakeDoudou();
                    m_uiManager.TakeObject();
                }
                else if (m_doudouIsPossessed == true)
                {
                    m_uiManager.DisableUi();
                }
            }
        }
    }


    private void OnTriggerExit(Collider p_collide)
    {
        if ((m_flashlightMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_uiManager.DisableUi();
        }
        else if ((m_doudouMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_uiManager.DisableUi();
        }
    }

    //----------------------------------------------- Fonctions correspondantes au doudou et à la lampe ------------------------------------------//

    //On pourrait les optis en vrais mais bon flm ?
    public void TakeFlashlight()
    {
        if (m_gameManager.isPc == true)
        {
            if (Input.GetKey(KeyCode.E))
            {
                m_uiManager.DisableUi();
                m_flm.PickItem();
                m_flashlightIsPossessed = true;
            }
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.buttonEast.isPressed)
            {
                m_uiManager.DisableUi();
                m_flm.PickItem();
                Gamepad.current.SetMotorSpeeds(0.75f * Time.deltaTime, 0.75f * Time.deltaTime);
                float frequency = InputSystem.pollingFrequency = 60f;
                m_flashlightIsPossessed = true;
            }
        } 
    }

    public void TakeDoudou()
    {
        if (m_gameManager.isPc == true)
        {
            if (Input.GetKey(KeyCode.E))
            {
                m_uiManager.DisableUi();
                m_doudouManager.PickItem();
                m_doudouIsPossessed = true;
            }
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.buttonEast.isPressed)
            {
                m_uiManager.DisableUi();
                m_doudouManager.PickItem();
                Gamepad.current.SetMotorSpeeds(0.75f * Time.deltaTime, 0.75f * Time.deltaTime);
                float frequency = InputSystem.pollingFrequency = 60f;
                m_doudouIsPossessed = true;
            }
        }
    }
    public void DropFlashlight()
    {
        if (m_gameManager.isPc == true)
        {
            if (Input.GetKey(KeyCode.G) && m_flashlightIsPossessed == true && m_doudouIsPossessed == false && isFullItem == false)
            {
                m_flm.DropItem();
                m_flashlightIsPossessed = false;
            }
           
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.buttonWest.isPressed && m_flashlightIsPossessed == true && m_doudouIsPossessed == false && isFullItem == false)
            {
                m_flm.DropItem();
                m_flashlightIsPossessed = false;
            }
        }
        if (m_flashlightIsPossessed == true && m_doudouIsPossessed == true && Input.GetKeyDown(KeyCode.G) && isFullItem == false)
        {
            FullItem();
        }

    }

    public void DropDoudou()
    {
        if (m_gameManager.isPc == true)
        {
            if (Input.GetKeyDown(KeyCode.G) && m_doudouIsPossessed == true && m_flashlightIsPossessed == false && isFullItem == false)
            {
                m_doudouManager.DropItem();
                m_doudouIsPossessed = false;
                Debug.Log("drop doudou");

            }
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.buttonWest.isPressed && m_doudouIsPossessed == true && m_flashlightIsPossessed == false && isFullItem == false)
            {
                m_doudouManager.DropItem();
                m_doudouIsPossessed = false;
            }
        }
    }

    public void FullItem()
    {
        isFullItem = true;
        if (isFullItem == true)
        {
            m_flm.DropItem();
            m_flashlightIsPossessed = false;
            Debug.Log("ça drop un des 2");

        }
    }

}
