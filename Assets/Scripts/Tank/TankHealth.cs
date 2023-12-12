using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : NetworkBehaviour
{
    public float m_StartingHealth = 100f;
    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public GameObject m_ExplosionPrefab;
    public Animator shieldAnimator;

    private AudioSource m_ExplosionAudio;
    private ParticleSystem m_ExplosionParticles;
    private float m_CurrentHealth;
    private bool m_Dead;

    public bool isPlayer;
    public bool hasShield;
    public Team team;

    public Action<TankHealth> OnDie;

    private float shieldTimer;


    public enum Team
    {
        Player,
        Enemy
    }

    private void Awake()
    {
        m_ExplosionParticles = Instantiate (m_ExplosionPrefab).GetComponent<ParticleSystem> ();

        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource> ();

        m_ExplosionParticles.gameObject.SetActive (false);
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }

    private void Update()
    {
        if (hasShield)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                shieldAnimator.SetTrigger("Disappear");
                hasShield = false;
            }
        }
    }

    public void Heal(float amount)
    {
        if (!IsServer) return;

        HealClientRpc(amount);
    }

    [ClientRpc]
    private void HealClientRpc(float amount)
    {
        m_CurrentHealth += amount;
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, m_StartingHealth);

        SetHealthUI();
    }

    public void Shield(float duration)
    {
        if (!IsServer) return;

        ShieldClientRpc(duration);
    }

    [ClientRpc]
    private void ShieldClientRpc(float duration)
    {
        shieldTimer = duration;

        if (!hasShield)
        {
            shieldAnimator.SetTrigger("Appear");
            hasShield = true;
        }
    }

    public void TakeDamage(TankHealth originHealth, float damage)
    {
        if (!IsServer) return;

        if (team == originHealth.team)
        {
            return;
        }

        if (hasShield)
        {
            return;
        }

        TankDamageClientRpc(damage);
    }

    [ClientRpc]
    private void TankDamageClientRpc(float damage)
    {
        m_CurrentHealth -= damage;

        SetHealthUI();

        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }

    private void SetHealthUI()
    {
        m_Slider.value = m_CurrentHealth;
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }

    private void OnDeath()
    {
        m_Dead = true;

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        m_ExplosionParticles.Play();

        m_ExplosionAudio.Play();

        OnDie?.Invoke(this);
    }
}