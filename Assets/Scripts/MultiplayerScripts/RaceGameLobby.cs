using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.VisualScripting;
using System;

public class RaceGameLobby : MonoBehaviour
{
    public static RaceGameLobby Instance { get; private set; }

    public event EventHandler OnCreateLobbyStarted;
    public event EventHandler OnCreateLobbyFailed;
    public event EventHandler OnJoinStarted;
    public event EventHandler OnQuickJoinFailed;
    public event EventHandler OnJoinFailed;
    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs { 
        public List<Lobby> lobbyList;
    }

    private Lobby joinedLobby;
    private bool isInitialized = false;
    private float heartBeatTimer;
    private float listLobbiesTimer;


    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeUnityAuthentication();
    }

    void Update()
    {
        HandleHeartBeat();
        HandlePeriodicListLobbies();
    }

    private void HandlePeriodicListLobbies()
    {
        if (joinedLobby == null && AuthenticationService.Instance.IsSignedIn)
        {
            listLobbiesTimer -= Time.deltaTime;
            if (listLobbiesTimer < 0)
            {
                float listLobbiesTimerMax = 5f;
                listLobbiesTimer = listLobbiesTimerMax;

                ListLobbies();
            }
        }
    }

    private void HandleHeartBeat()
    {
        if(IsLobbyHost())
        {
            heartBeatTimer -= Time.deltaTime;
            if (heartBeatTimer < 0)
            {
                float heartBeatTimerMax = 15f;
                heartBeatTimer = heartBeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private async void ListLobbies()
    {
        try {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
                Filters = new List<QueryFilter>{ 
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT) 
                }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync();
            
            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs {
                lobbyList = queryResponse.Results
            });
        }
        catch (LobbyServiceException e) {
            Debug.LogError(e.Message);
        }   
    }

    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(UnityEngine.Random.Range(0, 1000).ToString());

            await UnityServices.InitializeAsync(initializationOptions);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            isInitialized = true;
        }
    }

    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
        if (!isInitialized)
        {
            Debug.LogError("Unity Services are not initialized.");
            return;
        }

        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, RaceMultiplayerController.MAX_PLAYER_AMOUNT, new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
            });

            RaceMultiplayerController.Instance.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("SelectRace", LoadSceneMode.Single);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e.Message);
            OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public async void QuickJoin()
    {
        OnJoinStarted?.Invoke(this, EventArgs.Empty);
        if (!isInitialized)
        {
            Debug.LogError("Unity Services are not initialized.");
            return;
        }

        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            RaceMultiplayerController.Instance.StartClient();
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e.Message);
            OnQuickJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public async void JoinWithID(string lobbyId)
    {
        OnJoinStarted?.Invoke(this, EventArgs.Empty);
        try {
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

            RaceMultiplayerController.Instance.StartClient();
        }
        catch (LobbyServiceException e){
            Debug.LogError(e.Message);
            OnJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public async void JoinWithCode(string lobbyCode)
    {
        OnJoinStarted?.Invoke(this, EventArgs.Empty);
        try {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            RaceMultiplayerController.Instance.StartClient();
        }
        catch (LobbyServiceException e){
            Debug.LogError(e.Message);
            OnJoinFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public async void DeleteLobby()
    {
        if (joinedLobby != null)
        {
            try {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

                joinedLobby = null;
            }
            catch (LobbyServiceException e) {
                Debug.LogError(e.Message);
            }
        }        
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;
            }
            catch (LobbyServiceException e) {
                Debug.LogError(e.Message);
            }
        }   
    }

    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e) {
                Debug.LogError(e.Message);
            }
        }   
    }

    public Lobby GetLobby()
    {
        return joinedLobby;
    }

}
