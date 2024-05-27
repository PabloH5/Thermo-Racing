using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SelectRaceReady : NetworkBehaviour
{
    public static SelectRaceReady Instance { get; private set; }

    private Dictionary<ulong, bool> playerReadyDictionary;


    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong,bool>();
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                // This player is not ready
                allClientsReady = false;
                break;
            }
        }
        Debug.Log("AllClientsReady: " + allClientsReady);

        if(allClientsReady)
        {
            if(RaceMultiplayerController.Instance.ObtainStringRace() != null)
            {
                RaceGameLobby.Instance.DeleteLobby();
                NetworkManager.Singleton.SceneManager.LoadScene(RaceMultiplayerController.Instance.ObtainStringRace(), LoadSceneMode.Single);
            }
        }
    }
}
