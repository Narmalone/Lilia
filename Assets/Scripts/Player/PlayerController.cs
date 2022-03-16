using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Si le joueur ne peut pas monter les escalier il faut changer le step offset dans unity ou dans le code du chara controller

    [SerializeField]private CharacterController m_myChara;

    [SerializeField, Tooltip("Vitesse du joueur")]private float m_speed;

    private float m_gravity = -9.81f;

    Vector3 m_velocity;

    [SerializeField, Tooltip("Transform d'un empty ou sera cr�e la sphere pour savoir si le joueur est sur le sol")]private Transform groundCheck;

    [SerializeField, Tooltip("Radius de la sphere qui check si le joueur est sur le sol")]private float radiusCheckSphere = 0.4f;

    [SerializeField, Tooltip("Mask ou l'on d�finit le sol")]private LayerMask m_groundMask;

    private bool m_isGrounded;      //Si le joueur est sur le sol ?

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

        //Activation de la lampe
        ActiveFlashlight();
        ActiveDoudou();
    }

    //Variables, r�f�rences et fonctions de la lampe par rapport au joueur
    
    [SerializeField] FlashlightManager flm;
    
    [SerializeField] Doudou m_doudou;
    
    public bool flashlightIsPossessed = false;
    
    public bool m_doudouIsPossessed = false;
    public void TakeFlashlight()
    {
        if (Input.GetKey(KeyCode.E))
        {
            flm.PickItem();
            flashlightIsPossessed = true;
        }
    }
    
    public void TakeDoudou()
    {
        if (Input.GetKey(KeyCode.A))
        {
            m_doudou.PickItem();
            m_doudouIsPossessed = true;
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
        }

    }
    
    public void ActiveDoudou()
    {
        if (Input.GetKeyDown(KeyCode.F) && m_doudouIsPossessed == true)
        {
            m_doudou.UseDoudou();
        }
        if (Input.GetKey(KeyCode.G) && m_doudouIsPossessed == true)
        {
            m_doudou.DropItem();
            Debug.Log("Drop doudou");
            m_doudou.GetComponent<BoxCollider>().enabled = true;
            m_doudouIsPossessed = false;
        }

    }

}
