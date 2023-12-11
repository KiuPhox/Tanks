using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PowerupManager : NetworkBehaviour
{
    [SerializeField]
    private List<NetworkObject> powerupList;

    public static PowerupManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Spawn(Vector3 position)
    {
        if (!IsServer) return;

        NetworkObject powerupNetworkObject = Instantiate(powerupList[Random.Range(0, powerupList.Count)], position, Quaternion.identity);

        powerupNetworkObject.Spawn();
    }
}
