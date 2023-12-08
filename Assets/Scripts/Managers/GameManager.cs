using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TankManager.Instance.SpawnPlayerTank(Vector3.zero);

        TankManager.Instance.SpawnEnemyTank(new Vector3(5, 0, -10), Random.Range(0, 360f));
    }
}