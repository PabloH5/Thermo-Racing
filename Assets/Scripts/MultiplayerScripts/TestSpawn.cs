using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestSpawn : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                RaceController raceController = FindObjectOfType<RaceController>();
                if (raceController != null)
                {
                    Debug.Log($"My position actual is: {transform.position}");
                    transform.position = raceController.GetRandomSpawnPoint();
                    Debug.Log($"My position change to: {transform.position}");
                }
                else
                {
                    Debug.LogError("RaceController not found.");
                }
            }
            else
            {
                StartCoroutine(WaitAndFindRaceController());
            }
        }
    }

    private IEnumerator WaitAndFindRaceController()
    {
        RaceController raceController = null;
        while (raceController == null)
        {
            raceController = FindObjectOfType<RaceController>();
            yield return null; 
        }

        RequestSpawnPositionServerRpc();
    }

    [ServerRpc]
    private void RequestSpawnPositionServerRpc(ServerRpcParams rpcParams = default)
    {
        RaceController raceController = FindObjectOfType<RaceController>();
        if (raceController != null)
        {
            Vector3 spawnPosition = raceController.GetRandomSpawnPoint();
            SetSpawnPositionClientRpc(spawnPosition, rpcParams.Receive.SenderClientId);
        }
    }

    [ClientRpc]
    private void SetSpawnPositionClientRpc(Vector3 spawnPosition, ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            transform.position = spawnPosition;
            Debug.Log($"Player moved to: {transform.position}");
        }
    }
}
