using Pathfinding;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Vector2 powerupSpawnRange;
    private float powerupSpawnTimer;

    [SerializeField]
    private Vector2 enemySpawnRange;
    private float enemySpawnTimer;

    private RecastGraph grid;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        powerupSpawnTimer = Random.Range(powerupSpawnRange.x, powerupSpawnRange.y);
        enemySpawnTimer = Random.Range(enemySpawnRange.x, enemySpawnRange.y);

        grid = AstarPath.active.data.recastGraph;
    }

    private void Update()
    {
        if (!IsServer) return;

        if (enemySpawnTimer > 0)
        {
            enemySpawnTimer -= Time.deltaTime;
            if (enemySpawnTimer <= 0)
            {
                enemySpawnTimer = Random.Range(enemySpawnRange.x, enemySpawnRange.y);
                TankManager.Instance.SpawnEnemyTank(GetRandomPosition(), Random.Range(0, 360));
            }
        }

        if (powerupSpawnTimer > 0)
        {
            powerupSpawnTimer -= Time.deltaTime;
            if (powerupSpawnTimer <= 0)
            {
                powerupSpawnTimer = Random.Range(powerupSpawnRange.x, powerupSpawnRange.y);
                PowerupManager.Instance.Spawn(GetRandomPosition());
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position = new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
        var nn = grid.GetNearest(position);
        return (Vector3)nn.node.position;
    }

    public void EndGame()
    {
        EndGameClientRpc();
    }

    [ClientRpc]
    private void EndGameClientRpc()
    {
        UIManager.Instance.SetGameOverUI(true);
    }
}