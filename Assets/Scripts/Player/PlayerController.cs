using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMOD.Studio;
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
    [SerializeField] private AssetMenuScriptValue m_assetMenu;
    [SerializeField] private QTEManager m_qte;
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

    [SerializeField] public Camera m_cam;

    [SerializeField] private PortillonScript m_portillon;

    private NavMeshPath m_path;
    [Space(10)]
    [SerializeField] private LayerMask m_flashlightMask;
    [SerializeField] private LayerMask m_doudouMask;
    [SerializeField] private LayerMask m_TwoHandsItemMask;
    [SerializeField] private LayerMask m_iaMask;
    [SerializeField] private LayerMask m_portillonMask;
    [SerializeField] private LayerMask m_radioMask;
    [SerializeField] private LayerMask m_commodeMask;
    [SerializeField] private LayerMask m_keyMask;

    //-----------------------------------------------Sound System------------------------------------------//
    [Space(10)]
    [Header("System de son")]
    public FMODUnity.EventReference m_fmodEventPas;

    private EventInstance m_fmodInstancePas;

    public FMODUnity.EventReference m_fmodEventDoudou;

    private EventInstance m_fmodInstanceDoudou;

    public FMODUnity.EventReference m_fmodEventStress;

    private EventInstance m_fmodInstanceStress;

    [SerializeField] [Range(0, 10)] private float m_stressTest;

    private Vector3 previous;
    public float velocity;




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

    [NonSerialized] public Vector3 m_velocity;
    public Vector3 move;
    private bool m_isGrounded;

    public RaycastHit m_hit;

    private RaycastHit m_pastHit;

    public Ray m_ray;

    public bool isCinematic = true;

    [Space(10)]
    [SerializeField] private Renderer m_flashlightRend;
    [SerializeField] private Renderer m_doudouRend;
    [SerializeField] private Renderer m_doorRend;
    [SerializeField] private Renderer m_phoneRend;
    [SerializeField] private Renderer m_ChairRend;
    [Space(10)]
    public bool hasChair = false;
    public bool canInteractWithChair = false;
    public bool OnChair = false;
    [Space(10)]
    public bool isLeftHandFull = false;
    public bool isRightHandFull = false;
    public bool isTwoHandFull = false;

    public bool noNeedStress = false;
    public bool inCompteur = false;
    public bool isMoving = false;
    private float timeBeforeDropDoudou = 0.3f;
    private float timeBeforeDropVeilleuse = 0.3f;
    [SerializeField] private FinalScript m_final;
    [SerializeField] public Animator m_myAnim;
    [SerializeField] private Animator m_imgBlikImage;
    [SerializeField] private Transform m_StartingPlayerPosition;
    [SerializeField] private Transform m_AwakePlaterPosition;

    public bool m_stopStress = true;

    private void Awake()
    {
        isCinematic = true;
        isLeftHandFull = false;
        isRightHandFull = false;
        isTwoHandFull = false;
        isMoving = false;
        m_txtEvent.gameObject.SetActive(false);
        m_stopStress = true;
        noNeedStress = false;
        m_gameManager = FindObjectOfType<GameManager>();
        m_menuManager = FindObjectOfType<MenuManager>();
        m_controls = new PlayerControls();
        m_flm = FindObjectOfType<FlashlightManager>();
        m_path = new NavMeshPath();

        m_currentStress = m_maxStress;
        m_stressBar.SetMaxHealth(m_maxStress);

        m_camShake.camShakeActive = false;
        if (m_AIStateMachine == null)
        {
            return;
        }
        Debug.Log(m_linkedPostProcess.profile.TryGet(out m_dOFSettings));
        Debug.Log(m_linkedPostProcess.profile.TryGet(out m_vignetteSettings));

        m_fmodInstancePas = FMODUnity.RuntimeManager.CreateInstance(m_fmodEventPas);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_fmodInstancePas, GetComponent<Transform>(), GetComponent<Rigidbody>());
        //Debug.Log($"Démarage du son de pas : {m_fmodInstancePas.start()}");

        m_fmodInstanceStress = FMODUnity.RuntimeManager.CreateInstance(m_fmodEventStress);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_fmodInstanceStress, GetComponent<Transform>(), GetComponent<Rigidbody>());
        m_fmodInstanceStress.start();
        //m_fmodInstance.start();

        m_fmodInstanceDoudou = FMODUnity.RuntimeManager.CreateInstance(m_fmodEventDoudou);
        previous = Vector3.zero;

        hasChair = false;
        OnChair = false;
    }
    private void Start()
    {
        m_gameManager.PlayerInGame();
        bool startCinematic = false;
        if (startCinematic == false)
        {
            m_myAnim.SetTrigger("BeforeAwake");
            gameObject.transform.position = m_StartingPlayerPosition.position;
            gameObject.transform.rotation = m_StartingPlayerPosition.rotation;
            startCinematic = true;
            StartCoroutine(Faded());
            StartCoroutine(CoroutineAwake());
        }
    }
    IEnumerator CoroutineAwake()
    {
        yield return new WaitForSeconds(25f);
        gameObject.transform.position = m_AwakePlaterPosition.position;
        gameObject.transform.rotation = m_AwakePlaterPosition.rotation;
        m_myAnim.SetTrigger("AwakePlayer");

        yield return new WaitForSeconds(2f);
        isCinematic = false;
        m_stopStress = false;
        m_txtEvent.gameObject.SetActive(true);
        StopCoroutine(CoroutineAwake());
    }
    IEnumerator Faded()
    {
        m_imgBlikImage.SetBool("FadeActive", true);
        yield return new WaitForSeconds(0.7f);
        m_imgBlikImage.SetBool("FadeActive", false);
        yield return new WaitForSeconds(0.7f);
        m_imgBlikImage.SetBool("FadeActive", true);
        yield return new WaitForSeconds(0.7f);
        m_imgBlikImage.SetBool("FadeActive", false);
        yield return new WaitForSeconds(0.7f);
        m_imgBlikImage.SetBool("FadeActive", true);
        yield return new WaitForSeconds(0.7f);
        m_imgBlikImage.SetBool("FadeActive", false);
        yield return new WaitForSeconds(3f);
        m_imgBlikImage.SetBool("FadeActive", true);
        yield return new WaitForSeconds(1f);
        m_imgBlikImage.SetTrigger("EndAnim");
        yield return new WaitForSeconds(18f);
        m_imgBlikImage.gameObject.SetActive(false);
        StopCoroutine(Faded());
    }
    private void Update()
    {
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

        if (isCinematic == false)
        {
            m_myAnim.enabled = false;

            m_ray = m_cam.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(m_ray.origin, m_ray.direction, Color.black);
            if (Physics.Raycast(m_ray, out m_hit, 1f, ~(1 << gameObject.layer)))
            {
                //Debug.Log($"Je touche avec le raycast: {m_hit.collider.name}");
                OnRayCastHit(m_hit.collider);
                m_pastHit = m_hit;
            }
            else
            {
                if (m_pastHit.collider != null) OnRaycastExit(m_pastHit.collider);
            }

            //Debug.Log(m_hit.collider.name);

            velocity = ((transform.position - previous).magnitude) / Time.deltaTime;
            previous = transform.position;
            if (velocity < 0.2)
            {
                m_fmodInstancePas.setParameterByName("Speed", 0);
            }
            else
            {
                m_fmodInstancePas.setParameterByName("Speed", velocity * 3);
            }
            m_fmodInstancePas.setVolume(m_assetMenu.value);
            m_fmodInstanceStress.setVolume(m_assetMenu.value);
            m_fmodInstanceStress.setParameterByName("Stress", 10 - m_currentStress * 10 / m_maxStress);


            if (!m_stopStress) AutoStress();

            m_isGrounded = Physics.CheckSphere(groundCheck.position, radiusCheckSphere, m_groundMask);      //Cr�ation d'une sphere qui chech si le joueur touche le sol

            if (m_isGrounded && m_velocity.y < 0)        //Reset de la gravit� quand le joueur touche le sol
            {
                m_velocity.y = -2f;
            }

            if (inCompteur == false)
            {
                if (m_doudouIsUsed == false)
                {
                    PLAYBACK_STATE state;
                    m_fmodInstanceDoudou.getPlaybackState(out state);
                    if (state != PLAYBACK_STATE.STOPPED)
                    {
                        m_fmodInstanceDoudou.stop(STOP_MODE.ALLOWFADEOUT);
                    }

                    float x = Input.GetAxis("Horizontal");
                    float z = Input.GetAxis("Vertical");

                    move = transform.right * x + transform.forward * z;

                    m_myChara.Move(move * m_speed * Time.deltaTime);
                }
                if (m_doudouIsUsed == true)
                {
                    PLAYBACK_STATE state;
                    m_fmodInstanceDoudou.getPlaybackState(out state);
                    if (state == PLAYBACK_STATE.STOPPED)
                    {
                        FMODUnity.RuntimeManager.AttachInstanceToGameObject(m_fmodInstanceDoudou, GetComponent<Transform>(), GetComponent<Rigidbody>());
                        m_fmodInstanceDoudou.start();
                        m_fmodInstanceDoudou.setVolume(m_assetMenu.value);
                    }
                    float x = Input.GetAxis("Horizontal");
                    float z = Input.GetAxis("Vertical");

                    move = transform.right * x + transform.forward * z;

                    m_myChara.Move(move * m_speed * m_slow * Time.deltaTime);

                }
                // D�placements du joueur

                m_myChara.Move(m_velocity * Time.deltaTime);

                m_velocity.y += m_gravity * Time.deltaTime;

            }


            //Check Fonctions

            ActiveDoudou();
            Debug.Log(noNeedStress);
            // test shader
            // decay the target intensity
            if (noNeedStress == false)
            {
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
            }

            if (m_flashlightIsPossessed == true)
            {
                timeBeforeDropVeilleuse -= Time.deltaTime;
                if (timeBeforeDropVeilleuse <= 0f)
                {
                    timeBeforeDropVeilleuse = 0f;
                    DropFlashlight();
                }
            }
            if (m_doudouIsPossessed == true)
            {
                timeBeforeDropDoudou -= Time.deltaTime;
                if (timeBeforeDropDoudou <= 0f)
                {
                    timeBeforeDropDoudou = 0f;
                    DropDoudou();
                }
                if (m_doudouIsUsed == true)
                {
                    Stressing(-m_StressPower);
                }
            }

            //Si raycast avec portillon
            if (Physics.Raycast(m_ray, out m_hit, 1, m_portillonMask))
            {
                if (isLeftHandFull == false || isRightHandFull == false)
                {
                    if (isTwoHandFull == false)
                    {
                        m_doorRend = m_hit.collider.gameObject.GetComponent<Renderer>();
                        m_doorRend.material.SetFloat("_BooleanFloat", 1f);
                        m_UIManager.TakableObject();

                        if (Input.GetKey(KeyCode.E))
                        {
                            m_hit.collider.gameObject.GetComponent<Doors>().ActiveDoors();
                        }
                        if (m_hit.collider.gameObject.GetComponent<Doors>().isOpen == true)
                        {
                            m_doorRend.material.SetFloat("_BooleanFloat", 0f);
                            m_UIManager.StopRaycastBefore();
                            m_UIManager.DisableUi();
                        }
                    }
                    else if (isTwoHandFull == true)
                    {
                        m_UIManager.DropSomethingBefore();
                    }
                }
                if (isLeftHandFull == true && isRightHandFull == true)
                {
                    m_UIManager.DropSomethingBefore();
                }
            }
            else
            {
                m_doorRend.material.SetFloat("_BooleanFloat", 0f);
                FindObjectOfType<Doors>().DisableSlider();
            }


            if (Physics.Raycast(m_ray, out m_hit, 1, m_TwoHandsItemMask))
            {
                if (m_createNarrativeEvent.index == 3)
                {
                    if (m_phone.isFirstAnswer == true)
                    {
                        if (isTwoHandFull == false)
                        {
                            if (isLeftHandFull == true || isRightHandFull == true)
                            {
                                m_UIManager.NotChairTakable();
                            }

                            if (isLeftHandFull == false && isRightHandFull == false)
                            {
                                m_ChairRend = m_hit.collider.gameObject.GetComponent<Renderer>();
                                if (hasChair == false)
                                {
                                    m_gameManager.canDrop = true;
                                    m_ChairRend.material.SetFloat("_BooleanFloat", 1f);
                                    m_UIManager.TakableDoudou();
                                    if (Input.GetMouseButtonDown(0))
                                    {
                                        m_hit.collider.gameObject.GetComponent<CaisseProto>().CanTake();
                                    }
                                }
                                if (hasChair == true)
                                {
                                    m_gameManager.canDrop = false;
                                    m_UIManager.DisableUi();
                                    m_ChairRend.material.SetFloat("_BooleanFloat", 0f);
                                }
                                if (m_hit.collider.gameObject.GetComponent<CaisseProto>().isPlayerLocked == true)
                                {
                                    m_UIManager.DisableUi();
                                    m_ChairRend.material.SetFloat("_BooleanFloat", 0f);
                                }
                            }
                        }
                    }
                }
            }

            if (Physics.Raycast(m_ray, out m_hit, 1.5f, m_doudouMask))
            {
                if (isTwoHandFull == false)
                {
                    if (isLeftHandFull == false)
                    {
                        if (m_gameManager.canPick == true)
                        {
                            m_doudouRend.material.SetFloat("_BooleanFloat", 1f);
                            m_UIManager.TakableDoudou();
                            Debug.Log("float le material à true");
                            TakeDoudou();
                        }
                        else
                        {
                            m_doudouRend.material.SetFloat("_BooleanFloat", 1f);
                            m_UIManager.DisableUi();
                        }

                        if (m_hit.collider.gameObject.GetComponent<Renderer>() != null)
                        {
                            m_doudouRend = m_hit.collider.gameObject.GetComponent<Renderer>();
                        }
                    }
                }
            }

            if (Physics.Raycast(m_ray, out m_hit, 2f, m_radioMask))
            {
                if (OnChair == true)
                {
                    if (m_phone.isFirstAnswer == true)
                    {

                        if (m_createNarrativeEvent.index == 3)
                        {
                            m_phoneRend = m_hit.collider.gameObject.GetComponent<Renderer>();
                            m_UIManager.TakableObject();
                            m_phoneRend.material.SetFloat("_BooleanFloat", 1f);
                            Debug.Log("peut interagir");
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                if (m_doudouIsPossessed == true)
                                {
                                    m_doudouIsPossessed = false;
                                    m_doudou.DropItem();
                                    m_UIManager.DropDoudou();
                                    m_doudou.GetComponent<BoxCollider>().enabled = true;
                                    timeBeforeDropDoudou = 0.3f;
                                }
                                else if (m_doudouIsPossessed == false)
                                {
                                    NoNeedStress();
                                    isCinematic = true;
                                    m_phone.AnswerToCall();
                                    m_phone.isFirstAnswer = false;
                                    m_phoneRend.material.SetFloat("_BooleanFloat", 0f);
                                }

                            }
                        }

                    }
                    else
                    {
                        m_phoneRend.material.SetFloat("_BooleanFloat", 0f);
                    }
                }
                m_gameManager.canPick = false;

            }
            if (m_gameManager.gotKey == false)
            {
                if (Physics.Raycast(m_ray, out m_hit, 2f, m_keyMask))
                {
                    if (m_hit.collider.gameObject.GetComponent<KeyScript>())
                    {
                        m_hit.collider.gameObject.GetComponent<KeyScript>().CanTake();
                    }
                }
            }
        }
        else
        {
            m_myAnim.enabled = true;
        }


    }
    public void NoVelocity()
    {
        m_speed = 0;
    }
    public void NoNeedStress()
    {
        noNeedStress = true;
        m_intenseFieldOfView = 0;
        m_currentStress = 0f;
        m_fmodInstanceStress.setVolume(0);
    }
    /// <summary>
    /// Applique le stress au autre systeme manuellement
    /// </summary>
    /// <param name="p_stressNum">La quantité de stress appliqué</param>
    private void Stressing(float p_stressNum)
    {
        if (noNeedStress == false)
        {
            TakeDamage(p_stressNum);
            m_stressBar.SetStress(m_currentStress);
        }
    }

    /// <summary>
    /// Augmente le stress automatiquement (Dans l'update)
    /// </summary>
    public void AutoStress()
    {
        if (m_isStressTick == false)
        {
            TakeDamage(m_frequendeReduction);
            m_stressBar.SetStress(m_currentStress);
            m_isStressTick = true;
            Task.Delay(50).ContinueWith(t => m_isStressTick = false);
            Debug.Log("stress en cours");
        }
    }

    /// <summary>
    /// Modifie l'effet UI du stress sur le personnage selon une valeur donné
    /// </summary>
    /// <param name="amount">Quantité de "dégats"</param>
    public void TakeDamage(float amount)
    {
        if (m_gameManager.isPaused == true)
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
    private void OnRayCastHit(Collider p_collide)
    {
        if ((m_flashlightMask.value & (1 << p_collide.gameObject.layer)) > 0 && m_flashlightIsPossessed == false)
        {
            if (isTwoHandFull == false)
            {
                if (isRightHandFull == false)
                {
                    m_flashlightRend = m_hit.collider.gameObject.GetComponent<Renderer>();
                    m_flashlightRend.material.SetFloat("_BooleanFloat", 1f);
                    m_UIManager.TakableFlashlight();
                    TakeFlashlight();
                }
            }
        }
    }


    private void OnRaycastExit(Collider p_collide)
    {
        if ((m_flashlightMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_flashlightRend.material.SetFloat("_BooleanFloat", 0f);
            m_UIManager.DisableUi();
            m_UIManager.StopRaycastBefore();
        }
        else if ((m_doudouMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_doudouRend.material.SetFloat("_BooleanFloat", 0f);
            m_UIManager.DisableUi();
            m_UIManager.StopRaycastBefore();
        }
        else if ((m_portillonMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_UIManager.StopRaycastBefore();
            m_doorRend.material.SetFloat("_BooleanFloat", 0f);
            m_UIManager.DisableUi();
            m_gameManager.canPick = true;
        }
        else if ((m_radioMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_phoneRend.material.SetFloat("_BooleanFloat", 0f);
            m_UIManager.DisableUi();
            m_UIManager.StopRaycastBefore();
            m_gameManager.canPick = true;
        }
        else if ((m_TwoHandsItemMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_UIManager.StopRaycastBefore();
            m_ChairRend.material.SetFloat("_BooleanFloat", 0f);
            m_UIManager.DisableUi();
        }
        else if ((m_commodeMask.value & (1 << p_collide.gameObject.layer)) > 0)
        {
            m_UIManager.StopRaycastBefore();
            m_UIManager.DisableUi();
        }

    }
    //----------------------------------------------- Fonctions correspondantes au doudou et � la lampe ------------------------------------------//

    //Variables, r�f�rences et fonctions de la lampe par rapport au joueur
    /// </summary>
    public void TakeFlashlight()
    {
        if (m_gameManager.isPc == true)
        {
            if (m_gameManager.canDrop == true)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    if (m_flashlightIsPossessed == false)
                    {
                        if (m_createNarrativeEvent.isFirstTime == true && m_createNarrativeEvent.index == 2)
                        {
                            m_createNarrativeEvent.actionComplete = true;
                        }
                        if (m_createNarrativeEvent.index >= 2)
                        {
                            m_flashlightIsPossessed = true;
                            isRightHandFull = true;
                            m_flm.GetComponent<BoxCollider>().enabled = false;
                            m_UIManager.TakeLampe();
                            m_flm.PickItem();
                            m_UIManager.DisableUi();
                        }
                    }
                }
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
    /// <summary>
    /// Rassemble les fonctions pour le drop de la lampe
    /// </summary>
    public void DropFlashlight()
    {
        if (m_gameManager.isPc == true)
        {
            if (m_gameManager.canDrop == true)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    if (m_flashlightIsPossessed == true && timeBeforeDropVeilleuse <= 0f)
                    {
                        if (m_createNarrativeEvent.isFirstTime == true && m_createNarrativeEvent.index == 3)
                        {
                            m_phone.StartPhoneSound();
                            m_createNarrativeEvent.actionComplete = true;
                            m_txtEvent.gameObject.SetActive(false);
                        }
                        m_flm.DropItem();
                        m_flm.GetComponent<BoxCollider>().enabled = true;
                        isRightHandFull = false;
                        m_flashlightIsPossessed = false;
                        m_UIManager.DropLampe();
                        Debug.Log("Drop Light");
                        timeBeforeDropVeilleuse = 0.3f;
                    }
                }
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
    /// <summary>
    /// Rassemble les fonctions pour le ramassage du doudou
    /// </summary>
    public void TakeDoudou()
    {
        if (m_gameManager.isPc == true)
        {
            if (m_gameManager.canDrop == true)
            {
                if (m_doudouIsPossessed == false)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (m_createNarrativeEvent.isFirstTime == true && m_createNarrativeEvent.index == 0)
                        {
                            m_createNarrativeEvent.actionComplete = true;
                            m_createNarrativeEvent.isWaitingAction = false;
                        }
                        m_UIManager.TakeDoudou();
                        m_doudou.PickItem();
                        m_doudouIsPossessed = true;
                        isLeftHandFull = true;
                        //m_doudou.GetComponent<BoxCollider>().enabled = false;
                        m_UIManager.DisableUi();
                    }
                }
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
                //m_doudou.GetComponent<BoxCollider>().enabled = false;
                m_UIManager.DisableUi();
            }
        }
    }
    public void DropDoudou()
    {
        if (m_doudouIsPossessed == true && timeBeforeDropDoudou <= 0f)
        {
            timeBeforeDropDoudou = 0;
            if (m_gameManager.canDrop == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (m_createNarrativeEvent.isFirstTime == true && m_createNarrativeEvent.index == 3)
                    {
                        m_phone.StartPhoneSound();
                        m_createNarrativeEvent.actionComplete = true;
                        m_txtEvent.gameObject.SetActive(false);
                    }
                    m_doudou.DropItem();
                    m_doudouIsPossessed = false;
                    isLeftHandFull = false;
                    m_UIManager.DropDoudou();
                    //m_doudou.GetComponent<BoxCollider>().enabled = true;
                    timeBeforeDropDoudou = 0.3f;
                }
            }
        }

    }

    /// <summary>
    /// Rassemble les fonctions pour l'utilisation et le drop du doudou
    /// </summary>
    public void ActiveDoudou()
    {
        if (m_gameManager.isPc == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_doudouIsPossessed == true)
                {
                    m_doudouIsUsed = true;
                    //m_camShake.camShakeActive = true;

                    if (m_createNarrativeEvent.isFirstTime == true && m_createNarrativeEvent.index == 1)
                    {
                        m_doudouIsUsed = true;
                        m_createNarrativeEvent.actionComplete = true;
                    }
                    //Debug.Log("doit être chase");
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                m_doudouIsUsed = false;
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