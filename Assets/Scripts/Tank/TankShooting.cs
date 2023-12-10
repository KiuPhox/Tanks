using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public AudioSource m_ShootingAudio;
    public AudioClip m_FireClip;
    private TankHealth health;

    private void Awake()
    {
        health = GetComponent<TankHealth>();
    }
    public void Fire(float force)
    {
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation);

        shellInstance.GetComponent<ShellExplosion>().health = health;
        shellInstance.velocity = force * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
    }
}
