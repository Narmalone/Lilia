using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
using UnityEngine.AI;
using UnityEngine.EventSystems;

//----------------------------------------------- References from other Class ------------------------------------------//

public class PlayerController : MonoBehaviour
{
    //Si le joueur ne peut pas monter les escalier il faut changer le step offset dans unity ou dans le code du chara controller

    [SerializeField, Tooltip("Script de Control de l'UI")] private UiManager m_UIManager;

    [SerializeField] private CreateNarrativeEvent m_createNarrativeEvent;

    [SerializeField] private TxtEvent m_txtEvent;

    [SerializeField] private TimelinePlayerScript m_timePlayerScript;
    [SerializeField] private AudioManagerScript m_audioScript;
    private MenuManager m_menuManager;
    [SerializeField] private RespawnMe m_respawn;

    [SerializeField] private FunRadio m_phone;

    [SerializeField, Tooltip("Script du doudou")] Doudou m_doudou;

    [SerializeField, Tooltip("Script de la lampe torche")] private FlashlightManager m_flm;

    [NonSerialized]
    public bool m_flashlightIsPossessed = false;

    [NonSerialized]
    public bool m_doudouIsPossessed = false;

    [SerializeField, Tooltip("Script de la State Machine de l'IA")] AISM m_AIStateMachine;

    private bool m_doudouIsUsed = false;

    private GameObject m_target;
    
    //private DateTime startTime;

    public GameManager m_gameManager;

    public PlayerControls m_controls;

    [SerializeField] private PortillonScript m_portillon;

    private NavMeshPath m_path;
    [Space(10)]
    [SerializeField] private LayerMask m_flashlightMask;
    [SerializeField] private LayerMask m_doudouMask;
    [SerializeField] private LayerMask m_TwoHandsItemMask;
    [SerializeField] private LayerMask m_iaMask;
    [SerializeField] private LayerMask m_portillonMask;
    [SerializeField] private LayerMask m_radioMask;

    //-----------------------------------------------Sound System------------------------------------------//
  
    public FMODUnity.EventReference m_fmodEvent;

    private FMOD.Studio.EventInstance m_fmodInstance;

    [SerializeField] [Range(0,10)] private float m_speedFeet;

    

    //----------------------------------------------- Player controls system ------------------------------------------//

    [Header("System de controle joueur")]
    [SerializeField, Tooltip("R�f�rences du Chara controller")] private CharacterController m_myChara;


    [SerializeField, Tooltip("Vitesse du joueur en m/s")] public float m_speed;

    [SerializeField, Tooltip("puissance du stress en fonction du temps"), Range(0f, 1f)] private float m_StressPower;

    [SerializeField, Tooltip("Le joueur va stresser avec la touche espace en fonction de la valeur attribuée"), Range(0f, 10f)] private float m_makeMeStress;

    [SerializeField, Tooltip("Si la valeur est à 0.3 alors le joueur est slow de 70%"), Range(0f, 1f)] private float m_slow;


    //-----------------------------------------------Post-Processing------------------------------------------
    [Header("Post-Processing")]
    [SerializeField, Tooltip("Volume de post-process")] Volume m_linkedPostProcess;

    [SerializeField] private Material m_materialStress;

    [SerializeField, Tooltip("Script de Manager de Stress")] private StressManager m_stressBar;

    private float m_currentStress;

    private float m_targetStress;

    [SerializeField, Tooltip("Niveau de stress maximum")] float m_maxStress = 100f;

    private float m_targetIntensity = 0f;
    private float m_currentIntensity = 0f;

    [SerializeField, Tooltip("Intensité maximum de l'effet")] float m_intesiteMaxEffet = 0.5f;

    private Vignette m_vignetteSettings;

    private DepthOfField m_dOFSettings;

    private bool m_isStressTick = false;

    [SerializeField, Tooltip("Vitesse de réduction de l'effet de vignette")] float m_frequendeReduction = 0.2f;

    [SerializeField, Tooltip("Force d'attaque de l'effet")] float m_frequenceAttaque = 2f;
    [SerializeField, Tooltip("Vitesse de perte de l'attaque")] float m_frequenceRelache = 1f;

    [SerializeField]
    [Range(0f, 1f)] float m_intenseFieldOfView;

    [SerializeField, Tooltip("Courbe d'intensite de l'effet par rapport à la vie")] AnimationCurve m_intensityDueToHealth;
    [Space(10)]
    [SerializeField, Tooltip("Script de secouement")] private Shake m_camShake;

    //-----------------------------------------------Systeme Physics------------------------------------------
    [Header("Systeme de physique")]
    [SerializeField, Tooltip("Transform d'un empty ou sera cr�e la sphere pour savoir si le joueur est sur le sol")] private Transform groundCheck;

    [SerializeField, Tooltip("Radius de la sphere qui check si le joueur est sur le sol")] private float radiusCheckSphere = 0.4f;

    [SerializeField, Tooltip("Mask ou l'on d�finit le sol")] private LayerMask m_groundMask;

    private float m_gravity = -9.81f;

    Vector3 m_velocity;

    private bool m_isGrounded;

    private RaycastHit m_hit;
    private Ray m_ray;

    private void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_menuManager = FindObjectOfType<MenuManager>();
        m_controls = new PlayerControls();
        m_flm = FindObjectOfType<FlashlightManager>();
        m_path = new NavMeshPath();

        m_currentStress = m_maxStress;
        m_stressBar.SetMaxHealth(m_maxStress);

        m_camShake.camShakeActive = false;
        if(m_AIStateMachine == null)
        {
            return;
        }
        Debug.Log(m_linkedPostProcess.profile.TryGet(out m_dOFSettings));
        Debug.Log(m_linkedPostProcess.profile.TryGet(out m_vignetteSettings));

        m_fmodInstance = FMODUnity.RuntimeManager.CreateInstance(m_fmodEvent);
        Debug.Log(m_fmodInstance.start());
       
       
    }

    private void Update()
    {
        m_fmodInstance.setParameterByName("Speed", m_speedFeet);

        Debug.Log(m_doudouIsUsed);

        m_isGrounded = Physics.CheckSphere(groundCheck.position, radiusCheckSphere, m_groundMask);      //Cr�ation d'une sphere qui chech si le joueur touche le sol

        if (m_isGrounded && m_velocity.y < 0)        //Reset de la gravit� quand le joueur touche le sol
        {
            m_velocity.y = -2f;
        }

        if (m_doudouIsUsed == false)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            m_myChara.Move(move * m_speed * Time.deltaTime);
        }
        if (m_doudouIsUsed == true)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            m_myChara.Move(move * m_speed * m_slow * Time.deltaTime);
        }
        // D�placements du joueur

        m_myChara.Move(m_velocity * Time.deltaTime);

        m_velocity.y += m_gravity * Time.deltaTime;

        if (m_gameManager.isPc == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_gameManager.GamePaused();
                m_menuManager.OnPause();
            }
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                m_menuManager.OnPause();
                m_gameManager.GamePaused();
            }
        }


        //Check Fonctions

        AutoStress();
        ActiveFlashlight();
        ActiveDoudou();


        // test shader
        // decay the target intensity
        if (m_targetIntensity > 0f)
        {
            m_targetIntensity = Mathf.Clamp01(m_targetIntensity - m_frequendeReduction * Time.deltaTime);
            m_targetIntensity = Mathf.Max(m_intensityDueToHealth.Evaluate(m_currentStress / m_maxStress), m_targetIntensity);
        }

        // intensity needs updating
        if (m_currentIntensity != m_targetIntensity)
        {
            float rate = m_targetIntensity > m_currentIntensity ? m_frequenceAttaque : m_frequenceRelache;
            m_currentIntensity = Mathf.MoveTowards(m_currentIntensity, m_targetIntensity, rate * Time.deltaTime);
        }

        m_intenseFieldOfView = m_currentStress / 100;
        //Debug.Log(m_overlaySettings);
        m_materialStress.SetFloat("_Intensity", Mathf.Lerp(0f, 2f, m_currentIntensity));
        m_vignetteSettings.intensity.value = Mathf.Lerp(0f, m_intesiteMaxEffet, m_currentIntensity);
        m_dOFSettings.focusDistance.value = Mathf.Lerp(0.1f, 4f, m_intenseFieldOfView);

        if (m_doudouIsPossessed == true)
        {
            if (m_doudouIsUsed == true)
            {
                Stressing(-m_StressPower);
            }
            if (ReferenceEquals(m_target, gameObject) == false)
            {
                m_target = gameObject;
            }
        }
        else
        {
            if (ReferenceEquals(m_target,m_doudou.gameObject) == false)
            {
                m_target = m_doudou.gameObject;
            }
        }
        
        Chasse.GetPath(m_path, m_target.transform.position, m_AIStateMachine.transform.position, NavMesh.AllAreas);
        if (m_path.status == NavMeshPathStatus.PathComplete)
        {
            if (Chasse.GetPathLength(m_AIStateMachine.m_path) < 10f && m_AIStateMachine.currentState == m_AIStateMachine.m_chasseState)
            {
                float dist = Vector3.Distance(m_AIStateMachine.transform.position, m_doudou.transform.position);
                float power = dist / 10;
                float powerAdapted = Mathf.Lerp(0.1f, 0f, power);
                m_camShake.camShakeActive = true;
                
                Debug.Log("dans la chasse");
            }
            else
            {
                Debug.Log("ne doit plus chasser");
                m_camShake.camShakeActive = false;
            }
        }

       
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
            TakeDamage(m_frequendeReduction);
            m_stressBar.SetStress(m_currentStress);
            m_isStressTick = true;
            Task.Delay(50).ContinueWith(t => m_isStressTick = false);
        }
    }

    public void TakeDamage(float amount)
    {
        if(m_gameManager.isPaused == true)
        {
            m_maxStress = m_currentStress;
        }
        else
        {
            m_maxStress = 100f;
            m_currentStress = Mathf.Clamp(m_currentStress - amount, 0f, m_maxStress);

            float damagePercent = Mathf.Clamp01(amount / m_maxStress);

            m_targetIntensity = Mathf.Clamp01(m_targetIntensity + damagePercent);
        }
    }

    private void OnTriggerStay(Collider p_collide)
    {
        m_ray = Camera.main.ScreenPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if ((m_flashlightMask.value & (1 << p_collide.gameObject.layer)) > 0 && m_flashlightIsPossessed == false)
        {
            if(m_gameManager.gotKey == false)
            {
                if(m_gameManager.canPick == true)
                {
                    m_UIManager.TakableObject();
                    TakeFlashlight();
                }
                else
                {
                    m_UIManager.DisableUi();
                }
               
            }
            
        }

        else if ((m_doudouMask.value & (1 << p_collide.gameObject.layer)) > 0 && m_doudouIsPossessed == false)
        {
            if(m_gameManager.canPick == true)
            {
                m_UIManager.TakableObject();
                TakeDoudou();
            }
            else
            {
                m_UIManager.DisableUi();
            }
            
        }

        else if ((m_TwoHandsItemMask.value & (1 << p_collide.gameObject.layer)) > 0 && m_doudouIsPossessed == false && m_flashlightIsPossessed == false)
        {
            if (Physics.Raycast(m_ray, out m_hit, Mathf.Infinity, m_TwoHandsItemMask))
            {
                m_UIManager.TakableObject();

            }
            else
            {
                m_UIManager.DisableUi();
                return;
            }
        }

        else if ((m_portillonMask.value & (1 << p_collide.gameObject.layer)) > 0 && m_flashlightIsPossessed == false)
        {
            if (Physics.Raycast(m_ray, out m_hit, Mathf.Infinity, m_portillonMask))
            {
                m_UIManager.TakableObject();
                m_gameManager.canPick = true;
                if (m_gameManager.isPc == true)
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        m_portillon.UnlockPortillon();
                    }
                }
                else if (m_gameManager.isGamepad == true)
                {

                }
            }
            else
            {
                m_UIManager.DisableUi();
                return;
            }
        }
        else if ((m_radioMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            if (m_createNarrativeEvent.index == 1)
            {
                m_createNarrativeEvent.actionComplete = true;
            }
            m_UIManager.TakableObject();
            m_gameManager.canPick = false;
            if (m_gameManager.isPc == true)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    if (m_doudouIsPossessed == false && m_flashlightIsPossessed == false)
                    {
                        if (m_createNarrativeEvent.index == 2)
                        {
                            m_phone.AnswerToCall();
                            m_timePlayerScript.StartTimeline();
                        }
                    }
                }
            }
            else if (m_gameManager.isGamepad == true)
            {

            }
            
        }
    }


    private void OnTriggerExit(Collider p_collide)
    {
        if ((m_flashlightMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_UIManager.DisableUi();
        }
        else if ((m_doudouMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_UIManager.DisableUi();
        }
        else if ((m_portillonMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_UIManager.DisableUi();
            m_gameManager.canPick = true;
        }
        else if ((m_radioMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_UIManager.DisableUi();
            m_gameManager.canPick = true;
        }

    }
    //----------------------------------------------- Fonctions correspondantes au doudou et � la lampe ------------------------------------------//

    //Variables, r�f�rences et fonctions de la lampe par rapport au joueur

    public void TakeFlashlight()
    {
        if (m_gameManager.isPc == true)
        {
            if (Input.GetKey(KeyCode.E))
            {
                m_flashlightIsPossessed = true;
                m_flm.GetComponent<BoxCollider>().enabled = false;
                m_UIManager.TakeLampe();
                m_flm.PickItem();
                m_UIManager.DisableUi();
            }
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.buttonSouth.isPressed)
            {
                m_UIManager.TakeLampe();
                m_flm.PickItem();
                m_flashlightIsPossessed = true;
                Gamepad.current.SetMotorSpeeds(0.75f * Time.deltaTime, 0.75f * Time.deltaTime);
                float frequency = InputSystem.pollingFrequency = 60f;
                m_flm.GetComponent<BoxCollider>().enabled = false;
                m_UIManager.DisableUi();
            }
        }
    }

    public void TakeDoudou()
    {
        if (m_gameManager.isPc == true)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (m_createNarrativeEvent.isFirstTime == true && m_createNarrativeEvent.index == 0)
                {
                    m_createNarrativeEvent.actionComplete = true;
                    m_createNarrativeEvent.isWaitingAction = false;
                }
                m_UIManager.TakeDoudou();
                m_doudou.PickItem();
                m_doudouIsPossessed = true;
                m_doudou.GetComponent<BoxCollider>().enabled = false;
                m_UIManager.DisableUi();
            }
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.buttonSouth.isPressed)
            {
                m_UIManager.TakeDoudou();
                m_doudou.PickItem();
                Gamepad.current.SetMotorSpeeds(0.1f, 0.1f);
                InputSystem.pollingFrequency = 10f;
                m_doudouIsPossessed = true;
                m_doudou.GetComponent<BoxCollider>().enabled = false;
                m_UIManager.DisableUi();
            }
        }
    }

    public void ActiveFlashlight()
    {
        if (m_gameManager.isPc == true)
        {
            if (Input.GetKeyDown(KeyCode.G) && m_flashlightIsPossessed == true && m_doudouIsPossessed == false)
            {
                m_flm.DropItem();
                m_flm.GetComponent<BoxCollider>().enabled = true;
                m_flashlightIsPossessed = false;
                m_UIManager.DropLampe();
            }
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.buttonWest.isPressed && m_flashlightIsPossessed == true && m_doudouIsPossessed == false)
            {
                m_flm.DropItem();
                m_flm.GetComponent<BoxCollider>().enabled = true;
                m_flashlightIsPossessed = false;
                m_UIManager.DropLampe();
            }
        }

    }

    public void ActiveDoudou()
    {
        if (m_gameManager.isPc == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (m_doudouIsPossessed == true)
                {
                    m_doudouIsUsed = true;
                    //m_camShake.camShakeActive = true;

                    if (m_createNarrativeEvent.isFirstTime == true && m_createNarrativeEvent.index == 1)
                    {
                        m_doudouIsUsed = true;
                        m_createNarrativeEvent.actionComplete = true;
                        m_phone.StartPhoneSound();
                    }
                    //Debug.Log("doit être chase");
                }
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                Debug.Log("touche r non appuyé");
                m_doudouIsUsed = false;
            }
            if (Input.GetKeyDown(KeyCode.G) && m_doudouIsPossessed == true)
            {
                if (m_flashlightIsPossessed == false)
                {
                    m_doudou.DropItem();
                    m_doudouIsPossessed = false;
                    m_UIManager.DropDoudou();
                    Debug.Log("Drop doudou");
                    m_doudou.GetComponent<BoxCollider>().enabled = true;
                }
                else
                {
                    m_flm.DropItem();
                    m_flm.GetComponent<BoxCollider>().enabled = true;
                    m_flashlightIsPossessed = false;
                    m_UIManager.DropLampe();
                    Debug.Log("Drop Light");
                }

            }
        }
        else if (m_gameManager.isGamepad == true)
        {
            if (Gamepad.current.buttonWest.wasPressedThisFrame && m_doudouIsPossessed == true)
            {
                if (m_flashlightIsPossessed == false)
                {
                    m_doudou.DropItem();
                    m_doudouIsPossessed = false;
                    m_UIManager.DropDoudou();
                    Debug.Log("Drop doudou");
                    m_doudou.GetComponent<BoxCollider>().enabled = true;
                }
                else
                {
                    m_flm.DropItem();
                    m_flm.GetComponent<BoxCollider>().enabled = true;
                    m_flashlightIsPossessed = false;
                    m_UIManager.DropLampe();
                    Debug.Log("Drop Light");
                }
            }
        }

    }


}