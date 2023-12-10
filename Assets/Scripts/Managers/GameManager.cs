using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Update()
    {
        if (!IsServer) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TankManager.Instance.SpawnEnemyTank(new Vector3(0, 0, 10));
        }
    }

    public void EndGame()
    {

    }
}