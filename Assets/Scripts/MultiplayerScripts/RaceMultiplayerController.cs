using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class RaceMultiplayerController : NetworkBehaviour
{
    public static RaceMultiplayerController Instance { get; private set; }

    private int MAX_PLAYER_AMOUNT = 2;

    public EventHandler OnTryingToJoinGame;
    public EventHandler OnFailedToJoinGame;

    public enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    public NetworkVariable<float> countDownToStartTimer = new NetworkVariable<float>(3.0f);
    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);

    private bool _IsLocalPlayerReady { get; set; } = false;
    public event EventHandler OnLocalPlayerReadyChanged;
    public event EventHandler OnStateChanged;

    private Dictionary<ulong, bool> playerReadyDictionary;


    /// <summary>
    /// Handle disconnect in Pause and EndGame with 
    /// NetworkManager.Singleton.Shutdown()
    /// It is neccesary for do not break the logic
    /// </summary>

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        playerReadyDictionary = new Dictionary<ulong, bool>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Race")
        {
            LocalPlayerIsReady();
            if (IsServer)
            {
                Debug.Log("Host Ready");
            }
            else
            {
                Debug.Log("Client Ready");
            }
        }
    }

    public void Update()
    {
        if (!IsServer)
        {
            return;
        }

        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                countDownToStartTimer.Value -= Time.deltaTime;
                if (countDownToStartTimer.Value < 0f)
                {
                    state.Value = State.GamePlaying;
                }
                break;
            case State.GamePlaying:
                break;
            case State.GameOver:
                break;
        }
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        countDownToStartTimer.OnValueChanged += CountDownToStartTimer_OnValueChanged;
    }

    private void CountDownToStartTimer_OnValueChanged(float previousValue, float newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartHost()
    {
        Debug.Log("Starting host");
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_OnConnectionAprovalCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_OnConnectionAprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneManager.GetActiveScene().name != "CharacterSelectScene")
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "La carrera ya empezó";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClients.Count >= MAX_PLAYER_AMOUNT)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "La sala está llena";
            return;
        }

        connectionApprovalResponse.Approved = true;
    }

    public void StartClient()
    {
        Debug.Log("Starting client");
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
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

        if (allClientsReady)
        {
            state.Value = State.CountdownToStart;
            Debug.Log("Game state changed to CountdownToStart");
        }
    }

    public void LocalPlayerIsReady()
    {
        if (state.Value == State.WaitingToStart)
        {
            _IsLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsLocalPlayerReady()
    {
        return _IsLocalPlayerReady;
    }

    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state.Value == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }
}

