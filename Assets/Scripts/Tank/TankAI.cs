using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : MonoBehaviour
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
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            shootTimer = Random.Range(shootCooldownRange.x, shootCooldownRange.y);
            tankShooting.Fire(Random.Range(shootForceRange.x, shootForceRange.y));
        }
    }

    public void SetTarget(GameObject target)
    {
        aiDestinationSetter.target = target.transform;
    }
}
