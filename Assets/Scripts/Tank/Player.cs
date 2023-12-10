using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    private PlayerMovement playerMovement;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        TankManager.Instance.AddPlayerTank(GetComponent<TankHealth>());
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        playerMovement.Move();
        playerMovement.Turn();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        TankManager.Instance.RemovePlayerTank(GetComponent<TankHealth>());
    }
}
