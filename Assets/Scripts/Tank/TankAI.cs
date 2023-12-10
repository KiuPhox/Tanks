using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TankAI : NetworkBehaviour
{
    [SerializeField] private float sightRange;
    [SerializeField] private Vector2 shootForceRange;
    [SerializeField] private Vector2 shootCooldownRange;

    private TankShooting tankShooting;
    private AIDestinationSetter aiDestinationSetter;

    private float shootTimer;

    private void Awake()
    {
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        tankShooting = GetComponent<TankShooting>();
        shootTimer = Random.Range(shootCooldownRange.x, shootCooldownRange.y);
    }

    void Update()
    {
        if (!IsServer) return;

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            shootTimer = Random.Range(shootCooldownRange.x, shootCooldownRange.y);
            FireClientRpc(Random.Range(shootForceRange.x, shootForceRange.y));
        }
    }

    [ClientRpc]
    private void FireClientRpc(float force)
    {
        tankShooting.Fire(force);
    }

    public void SetTarget(GameObject target)
    {
        if (!IsServer) return;

        aiDestinationSetter.target = target.transform;
    }
}
