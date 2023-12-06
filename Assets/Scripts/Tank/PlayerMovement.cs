﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{  
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
	public float m_PitchRange = 0.2f;

    private Rigidbody m_Rigidbody;
    private float m_OriginalPitch;
    private ParticleSystem[] m_particleSystems;

    private float m_MovementInputValue;
    private float m_TurnInputValue;

    private void Awake ()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;

        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < m_particleSystems.Length; ++i)
        {
            m_particleSystems[i].Play();
        }
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;

        for(int i = 0; i < m_particleSystems.Length; ++i)
        {
            m_particleSystems[i].Stop();
        }
    }


    private void Start ()
    {
        m_OriginalPitch = m_MovementAudio.pitch;
    }


    private void Update ()
    {
        m_MovementInputValue = InputManager.Instance.GetMovementVector().y;
        m_TurnInputValue = InputManager.Instance.GetMovementVector().x;

        EngineAudio();
    }


    private void EngineAudio ()
    {
        if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play ();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... change the clip to driving and play.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate ()
    {
        Move();
        Turn();
    }


    private void Move()
    {
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn ()
    {
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

        m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }
}