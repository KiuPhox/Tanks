using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Powerup : NetworkBehaviour
{
    public enum PowerupType
    {
        Health,
        Speed,
        Damage,
        Shield
    }

    public PowerupType powerupType;
    private NetworkObject networkObject;
    private AudioSource audioSource;
    

    private void Awake()
    {
        networkObject = GetComponent<NetworkObject>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOwner && other.CompareTag("Player"))
        {
            OnPickup(other.GetComponent<TankHealth>());
        }
    }

    public void OnPickup(TankHealth tank)
    {
        switch (powerupType)
        {
            case PowerupType.Health:
                tank.Heal(50);
                break;
            case PowerupType.Speed:
                tank.GetComponent<PlayerMovement>().SetDoubleSpeedTimer(5);
                break;
            case PowerupType.Shield:
                tank.Shield(5);
                break;
        }

        audioSource.Play();

        DespawnServerRpc();
    }

    [ServerRpc]
    public void DespawnServerRpc()
    {
        networkObject.Despawn(true);
    }
}
