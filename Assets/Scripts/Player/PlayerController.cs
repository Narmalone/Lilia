using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using Unity.VisualScripting;
using System.Threading.Tasks;
using DamageOverlayEffect;
    //----------------------------------------------- References from other Class ------------------------------------------//

public class PlayerController : MonoBehaviour
{
    //Si le joueur ne peut pas monter les escalier il faut changer le step offset dans unity ou dans le code du chara controller

    [SerializeField] private UiManager m_UIManager;
    
    [SerializeField] FlashlightManager flm;
    
    [SerializeField] Doudou m_doudou;
    
    public bool flashlightIsPossessed = false;
    
    public bool m_doudouIsPossessed = false;

    [SerializeField] AIController m_AIController;

    private bool m_doudouIsUsed = false;

    private DateTime startTime;

    //-----------------------------------------------Systeme Stress------------------------------------------
    
    [SerializeField] private StressManager m_stressBar;
    
    private float m_currentStress;
    
    private float m_targetStress;

    [SerializeField] float m_maxStress = 100f;


    [SerializeField] FlashlightManager m_flm;
    DoudouManager m_doudouManager;

    [SerializeField] private LayerMask m_flashlightMask;
    [SerializeField] private LayerMask m_doudouMask;

    //----------------------------------------------- Player controls system ------------------------------------------//


    [SerializeField, Tooltip("R�f�rences du Chara controller")] private CharacterController m_myChara;


    [SerializeField, Tooltip("Vitesse du joueur en m/s")]private float m_speed;

    private bool m_isStressTick = false;

    [SerializeField] float DecayRate = 0.2f;
    
    [SerializeField] float AttackRate = 2f;
    [SerializeField] float ReleaseRate = 1f;

    [Range(0f, 1f)] float m_intenseFieldOfView;
    
    [SerializeField] AnimationCurve IntensityDueToHealth;
    
    //-----------------------------------------------Post-Processing------------------------------------------
    [SerializeField] PostProcessVolume LinkedPPV;
    
    float TargetIntensity = 0f;
    float CurrentIntensity = 0f;
    [SerializeField] float MaxEffectIntensity = 0.5f;
    
    DamageOverlay OverlaySettings;

    private DepthOfField m_dOFSettings;

    [SerializeField] private Shake m_camShake;

    //-----------------------------------------------Systeme Physics------------------------------------------
    
    [SerializeField]private CharacterController m_myChara;

    [SerializeField, Tooltip("Vitesse du joueur")]private float m_speed;

    private float m_gravity = -9.81f;
    
    Vector3 m_velocity;
    
    [SerializeField, Tooltip("Transform d'un empty ou sera cr�e la sphere pour savoir si le joueur est sur le sol")]private Transform groundCheck;
    
    //----------------------------------------------- Player Items Systems ------------------------------------------//

    public bool m_flashlightIsPossessed = false;
    public bool m_doudouIsPossessed = false;
    public bool isFullItem = false;
    //----------------------------------------------- Post-processing ------------------------------------------//


    [SerializeField, Tooltip("Radius de la sphere qui check si le joueur est sur le sol")]private float radiusCheckSphere = 0.4f;


    [SerializeField, Tooltip("Mask ou l'on d�finit le sol")]private LayerMask m_groundMask;
    
    private bool m_isGrounded;//Si le joueur est sur le sol ?
    
    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_controls = new PlayerControls();
        m_doudouManager = FindObjectOfType<DoudouManager>();
        m_flm = FindObjectOfType<FlashlightManager>();

        m_currentStress = m_maxStress;
        m_stressBar.SetMaxHealth(m_maxStress);
        Debug.Log(LinkedPPV.profile.TryGetSettings<DamageOverlay>(out OverlaySettings));
        Debug.Log(LinkedPPV.profile.TryGetSettings<DepthOfField>(out m_dOFSettings));
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
        // test shader
        // decay the target intensity
        if (TargetIntensity > 0f)
        {
            TargetIntensity = Mathf.Clamp01(TargetIntensity - DecayRate * Time.deltaTime);
            TargetIntensity = Mathf.Max(IntensityDueToHealth.Evaluate(m_currentStress / m_maxStress), TargetIntensity);
        }

        // intensity needs updating
        if (CurrentIntensity != TargetIntensity)
        {
            float rate = TargetIntensity > CurrentIntensity ? AttackRate : ReleaseRate;
            CurrentIntensity = Mathf.MoveTowards(CurrentIntensity, TargetIntensity, rate * Time.deltaTime);
        }

        m_intenseFieldOfView = m_currentStress / 100;
        //Debug.Log(OverlaySettings);
        OverlaySettings.Intensity.value = Mathf.Lerp(0f, MaxEffectIntensity, CurrentIntensity);
        m_dOFSettings.focusDistance.value = Mathf.Lerp(0.1f, 4f, m_intenseFieldOfView);

        if (Vector3.Distance(m_AIController.transform.position,m_doudou.transform.position)<10)
        {
            float dist = Vector3.Distance(m_AIController.transform.position, m_doudou.transform.position);
            float power = dist/10 ;
            float powerAdapted = Mathf.Lerp(0.1f, 0f,power);
            Debug.Log(powerAdapted);
            m_camShake.StartShake(0.15f,powerAdapted);
        }


        //Check si le joueur drop des items
        DropFlashlight();
        DropDoudou();
    }
    private void Stressing(float p_stressNum)
    {
        TakeDamage(p_stressNum);
        m_stressBar.SetStress(m_currentStress);
    }

    public void AutoStress()
    {
        if (m_isStressTick == false)
        {
            TakeDamage(DecayRate);
            m_stressBar.SetStress(m_currentStress);
            m_isStressTick = true;
            Task.Delay(50).ContinueWith(t=> m_isStressTick=false);
        }
    }
    
    public void TakeDamage(float amount)
    {
        m_currentStress = Mathf.Clamp(m_currentStress - amount, 0f, m_maxStress);

        float damagePercent = Mathf.Clamp01(amount / m_maxStress);

        TargetIntensity = Mathf.Clamp01(TargetIntensity + damagePercent);
    }

    private void OnTriggerStay(Collider p_collide)
    {
        if (m_gameManager.isPc)
        {

            if ((m_flashlightMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {

                if (m_flashlightIsPossessed == false)
                {
                    TakeFlashlight();
                    m_uiManager.UiTakeFlashlight();
                }
                else if (m_flashlightIsPossessed == true)
                {
                    m_uiManager.UiDisableFlashlight();
                }
            }

            else if ((m_doudouMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {

                if (m_doudouIsPossessed == false)
                {
                    TakeDoudou();
                    Debug.Log("prend le doudou");
                    m_uiManager.UiTakeDoudou();
                }
                else if (m_doudouIsPossessed == true)
                {
                    m_uiManager.UiTakeDoudou();
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
                    m_uiManager.UiTakeFlashlight();
                    Debug.Log("gamepad ui activ�e");
                }
                else if (m_flashlightIsPossessed == true)
                {
                    m_uiManager.UiDisableFlashlight();
                }
            }
            else if ((m_doudouMask.value & (1 << p_collide.gameObject.layer)) > 0)
            {

                if (m_doudouIsPossessed == false)
                {
                    TakeDoudou();
                    m_uiManager.UiTakeDoudou();
                }
                else if (m_doudouIsPossessed == true)
                {
                    m_uiManager.UiDisableDoudou();
                }
            }
        }
    }


    private void OnTriggerExit(Collider p_collide)
    {
        if ((m_flashlightMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_uiManager.UiDisableFlashlight();
        }
        else if ((m_doudouMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_uiManager.UiDisableDoudou();
        }
    }

    //----------------------------------------------- Fonctions correspondantes au doudou et � la lampe ------------------------------------------//

    //On pourrait les optis en vrais mais bon flm ?
    

    
    //Variables, r�f�rences et fonctions de la lampe par rapport au joueur

    public void TakeFlashlight()
    {
        if (Input.GetKey(KeyCode.E))
        {
            m_uiManager.UiDisableFlashlight();
            m_flm.PickItem();
            m_flashlightIsPossessed = true;
            
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
            if (Gamepad.current.buttonEast.isPressed)
            {
                m_uiManager.UiDisableFlashlight();
                m_flm.PickItem();
                Gamepad.current.SetMotorSpeeds(0.75f, 0.75f);
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
                m_uiManager.UiDisableDoudou();
                m_doudouManager.PickItem();
                m_doudouIsPossessed = true;
            }
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.buttonEast.isPressed)
            {
                m_uiManager.UiDisableDoudou();
                m_doudouManager.PickItem();
                Gamepad.current.SetMotorSpeeds(0.75f * Time.deltaTime, 0.75f * Time.deltaTime);
                float frequency = InputSystem.pollingFrequency = 60f;
                m_doudouIsPossessed = true;
            }
        }
    }
    public void ActiveFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F) && flashlightIsPossessed == true)
        {
            if (Input.GetKey(KeyCode.G) && m_flashlightIsPossessed == true && m_doudouIsPossessed == false && isFullItem == false)
            {
                m_flm.DropItem();
                m_flashlightIsPossessed = false;
            }

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
            if (Gamepad.current.buttonWest.isPressed && m_flashlightIsPossessed == true &&
                m_doudouIsPossessed == false && isFullItem == false)
            {
                m_flm.DropItem();
                m_flashlightIsPossessed = false;
            }

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
                Debug.Log("Gros Son");
                Stressing(-10);
                if (Vector3.Distance(m_AIController.transform.position, m_doudou.transform.position) < 10)
                {
                    m_AIController.FollowDoudou(1000);
                }
                
                
            }
            else if (pressedTime.TotalMilliseconds < 2000 )
            {
                Debug.Log("Piti Soin");
                Stressing(-20);
                if (Vector3.Distance(m_AIController.transform.position, m_doudou.transform.position) < 10)
                {
                    m_AIController.FollowDoudou(2000);
                }
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
            Debug.Log("�a drop un des 2");

        }
    }
}
