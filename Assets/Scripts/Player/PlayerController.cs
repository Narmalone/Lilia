using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Si le joueur ne peut pas monter les escalier il faut changer le step offset dans unity ou dans le code du chara controller

    [SerializeField]private CharacterController m_myChara;

    [SerializeField, Tooltip("Vitesse du joueur")]private float m_speed;

    private float m_gravity = -9.81f;

    [SerializeField] private StressManager m_stressBar;

    [SerializeField] private UiManager m_UIManager;
    
    private int m_currentStress;

    private int m_maxStress = 100;

    private bool m_isStressTick = false;

    private int m_tickStress = 5;

    private bool m_doudouIsUsed = false;

    private DateTime startTime;
    
    Vector3 m_velocity;

    [SerializeField, Tooltip("Transform d'un empty ou sera cr�e la sphere pour savoir si le joueur est sur le sol")]private Transform groundCheck;

    [SerializeField, Tooltip("Radius de la sphere qui check si le joueur est sur le sol")]private float radiusCheckSphere = 0.4f;

    [SerializeField, Tooltip("Mask ou l'on d�finit le sol")]private LayerMask m_groundMask;
    
    [SerializeField] FlashlightManager flm;
    
    [SerializeField] Doudou m_doudou;
    
    public bool flashlightIsPossessed = false;
    
    public bool m_doudouIsPossessed = false;

    private bool m_isGrounded;//Si le joueur est sur le sol ?
    
    [SerializeField] AIController m_AIController;

    private void Awake()
    {
        m_currentStress = m_maxStress;
        m_stressBar.SetMaxHealth(m_maxStress);
    }

    private void Update()
    {
        m_isGrounded = Physics.CheckSphere(groundCheck.position, radiusCheckSphere, m_groundMask);      //Cr�ation d'une sphere qui chech si le joueur touche le sol

        if(m_isGrounded && m_velocity.y < 0)        //Reset de la gravit� quand le joueur touche le sol
        {
            m_velocity.y = -2f;
        }
        

        // D�placements du joueur
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        m_myChara.Move(move * m_speed * Time.deltaTime);

        m_myChara.Move(m_velocity * Time.deltaTime);

        m_velocity.y += m_gravity * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Stressing(10);
        }
        
        AutoStress();
        //Activation de la lampe
        ActiveFlashlight();
        ActiveDoudou();
    }

    private void Stressing(int p_stressNum)
    {
        if (m_currentStress < 0)
        {
            m_currentStress = 0;
        }
        else if (m_currentStress > m_maxStress)
        {
            m_currentStress = m_maxStress;
        }
        else
        {
            m_currentStress -= p_stressNum;
        }
        m_stressBar.SetStress(m_currentStress);
    }

    public void AutoStress()
    {
        if (m_isStressTick == false)
        {
            if (m_currentStress < 0)
            {
                m_currentStress = 0;
            }
            else if (m_currentStress > m_maxStress)
            {
                m_currentStress = m_maxStress;
            }
            else
            {
                m_currentStress -= m_tickStress;
            }
            m_stressBar.SetStress(m_currentStress);
            m_isStressTick = true;
            Task.Delay(1000).ContinueWith(t=> m_isStressTick=false);
        }
    }
    
    //Variables, r�f�rences et fonctions de la lampe par rapport au joueur
    
    public void TakeFlashlight()
    {
        if (Input.GetKey(KeyCode.E))
        {
            flm.PickItem();
            flashlightIsPossessed = true;
            m_UIManager.TakeLampe();
        }
    }
    
    public void TakeDoudou()
    {
        if (Input.GetKey(KeyCode.A))
        {
            m_doudou.PickItem();
            m_doudouIsPossessed = true;
            m_UIManager.TakeDoudou();
        }
    }
    public void ActiveFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F) && flashlightIsPossessed == true)
        {
            flm.UseFlashlight();
        }
        if (Input.GetKey(KeyCode.G) && flashlightIsPossessed == true)
        {
            flm.DropItem();
            flm.GetComponent<BoxCollider>().enabled = true;
            flashlightIsPossessed = false;
            m_UIManager.DropLampe();
            m_UIManager.DisableUi();
            
        }

    }
    
    public void ActiveDoudou()
    {
        if (Input.GetKeyDown(KeyCode.R) && m_doudouIsPossessed == true)
        {
            startTime = DateTime.Now;
            m_doudou.UseDoudou();
            m_doudouIsUsed = true;
        }
        if (Input.GetKeyUp(KeyCode.R) && m_doudouIsUsed == true)
        {
            DateTime endTime = DateTime.Now;
            TimeSpan pressedTime = endTime.Subtract(startTime);
            if (pressedTime.TotalMilliseconds >= 2000)
            {
                Debug.Log("Gros Soin");
                Stressing(-10);
                m_AIController.FollowDoudou(1000);
                
            }
            else if (pressedTime.TotalMilliseconds < 2000 )
            {
                Debug.Log("Piti Soin");
                Stressing(-20);
                m_AIController.FollowDoudou(2000);
            }

            m_doudouIsUsed = false;
        }
        if (Input.GetKey(KeyCode.G) && m_doudouIsPossessed == true)
        {
            m_doudou.DropItem();
            Debug.Log("Drop doudou");
            m_doudou.GetComponent<BoxCollider>().enabled = true;
            m_doudouIsPossessed = false;
            m_UIManager.DropDoudou();
            m_UIManager.DisableUi();
        }
    }
}
