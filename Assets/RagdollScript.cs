using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollScript : MonoBehaviour
{

    private Rigidbody m_playerRb;
    private Collider[] m_bonesCollider;
    private Rigidbody[] m_bones;
    [SerializeField] Doudou m_doudou;
    private void Awake()
    {
        m_playerRb = m_doudou.m_rbDoudou;
        m_bones = GetComponentsInChildren<Rigidbody>();
        m_bonesCollider = GetComponentsInChildren<Collider>();
    }
    IEnumerator OnGround()
    {
        yield return new WaitForSeconds(0.5f);
        DisableRagdoll();
    }
    public void ActivateRagdoll()
    {
        m_playerRb.isKinematic = true;
        m_playerRb.useGravity = false;

        SetCollidersEnable(true);
        SetRigidBodiesKinematic(false);
    }
    public void DisableRagdoll()
    {
        StopCoroutine(OnGround());
        m_playerRb.isKinematic = true;
        m_playerRb.useGravity = false;

        SetCollidersEnable(false);
        SetRigidBodiesKinematic(true);

        gameObject.transform.position = m_doudou.transform.position;
    }
    public void SetCollidersEnable(bool enabled)
    {
        foreach (Collider p_bones in m_bonesCollider)
        {
            p_bones.enabled = enabled;
        }
    }
    public void SetRigidBodiesKinematic(bool kinematic)
    {
        foreach (Rigidbody p_rbody in m_bones)
        {
            p_rbody.isKinematic = kinematic;
        }
    }
}
