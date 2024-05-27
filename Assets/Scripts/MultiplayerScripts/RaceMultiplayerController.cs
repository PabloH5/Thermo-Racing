using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceMultiplayerController : NetworkBehaviour
{
    public static RaceMultiplayerController Instance { get; private set; }
    public static bool playMultiplayer;

    public const int MAX_PLAYER_AMOUNT = 2;
    private const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";

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

    private Button track1Button;
    private bool _IsTrack1Selected = false;
    private Button track2Button;
    private bool _IsTrack2Selected = false;

    public event EventHandler OnLocalPlayerReadyChanged;
    public event EventHandler OnStateChanged;

    private Dictionary<ulong, bool> playerReadyDictionary;

    private string playerName;

    /// <summary>
    /// Handle disconnect in Pause and EndGame with 
    /// NetworkManager.Singleton.Shutdown()
    /// It is neccesary for do not break the logic
    /// </summary>

    private void Awake()
    {
        Instance = this;
        playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName " + UnityEngine.Random.Range(100, 1000));

        DontDestroyOnLoad(gameObject);

        playerReadyDictionary = new Dictionary<ulong, bool>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public void SetPlayerName(string newPlayerName)
    {
        this.playerName = newPlayerName;

        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, newPlayerName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SelectRace")
        {
            track1Button = GameObject.Find("Track1Button").GetComponent<Button>();
            track2Button = GameObject.Find("Track2Button").GetComponent<Button>();

            track1Button.onClick.AddListener(() =>
            {
                _IsTrack1Selected = true;
            });
            track2Button.onClick.AddListener(() =>
            {
                _IsTrack2Selected = true;
            });
        }


        if (scene.name == "Track1Core" || scene.name == "Track2")
        {
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

    public string ObtainStringRace()
    {
        if (_IsTrack1Selected)
        {
            return "Track1Core";
        }
        if (_IsTrack2Selected)
        {
            return "Track2";
        }

        return null;
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        countDownToStartTimer.OnValueChanged += CountDownToStartTimer_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void CountDownToStartTimer_OnValueChanged(float previousValue, float newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartHost()
    {
        if (playMultiplayer)
        {
            Debug.Log("Starting host");
            NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_OnConnectionAprovalCallback;
            NetworkManager.Singleton.StartHost();
        }
    }

    private void NetworkManager_OnConnectionAprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneManager.GetActiveScene().name != "SelectRace")
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

    public void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);

        if (playerReadyDictionary.ContainsKey(clientId))
        {
            playerReadyDictionary.Remove(clientId);
        }

        if (NetworkManager.Singleton.IsServer)
        {
            if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
            {
                NetworkObject networkObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
                if (networkObject != null)
                {
                    networkObject.Despawn();
                    Destroy(networkObject.gameObject);
                }
            }
        }
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

    public override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        state.OnValueChanged -= State_OnValueChanged;
        countDownToStartTimer.OnValueChanged -= CountDownToStartTimer_OnValueChanged;
    }
}

