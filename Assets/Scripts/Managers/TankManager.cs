using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TankManager : NetworkBehaviour
{
    [SerializeField] private TankHealth playerTankPrefab;
    [SerializeField] private List<TankHealth> enemyTankPrefabs;

    public static TankManager Instance { get; private set; }
    public List<TankHealth> AllTanks { get; private set; }
    public List<TankHealth> PlayerTanks { get; private set; }
    public List<TankHealth> EnemyTanks { get; private set; }

    private void Awake()
    {
        Instance = this;

        AllTanks = new List<TankHealth>();
        PlayerTanks = new List<TankHealth>();
        EnemyTanks = new List<TankHealth>();
    }

    public void AddPlayerTank(TankHealth tank)
    {
        tank.OnDie += OnPlayerTankDie;

        AllTanks.Add(tank);
        PlayerTanks.Add(tank);
        CameraControl.Instance.AddTarget(tank.gameObject);
    }

    public void RemovePlayerTank(TankHealth tank)
    {
        OnPlayerTankDie(tank);
    }

    public void SpawnEnemyTank(Vector3 position, float angle = 0)
    {
        TankHealth enemyTankPrefab = enemyTankPrefabs[Random.Range(0, enemyTankPrefabs.Count)];
        TankHealth tank = Instantiate(enemyTankPrefab, position, Quaternion.identity);

        tank.GetComponent<NetworkObject>().Spawn();

        tank.GetComponent<TankAI>().SetTarget(PlayerTanks[0].gameObject);

        tank.transform.eulerAngles = new Vector3(0, angle, 0);
        tank.OnDie += OnEnemyTankDie;

        AllTanks.Add(tank);
        EnemyTanks.Add(tank);
    }
    
    private void OnPlayerTankDie(TankHealth tank)
    {
        AllTanks.Remove(tank);
        PlayerTanks.Remove(tank);

        tank.OnDie -= OnPlayerTankDie;
        CameraControl.Instance.RemoveTank(tank.gameObject);

        Destroy(tank.gameObject);

        if (PlayerTanks.Count == 0)
        {
            GameManager.Instance.EndGame();
        }
    }

    private void OnEnemyTankDie(TankHealth tank)
    {
        AllTanks.Remove(tank);
        EnemyTanks.Remove(tank);

        tank.OnDie -= OnEnemyTankDie;

        tank.GetComponent<NetworkObject>().Despawn();
    }
}