using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    void Start()
    {
        RaceMultiplayerController.Instance.OnFailedToJoinGame += RaceMultiplayer_OnFailedToJoinGame;
        RaceGameLobby.Instance.OnCreateLobbyStarted += RaceGameLobby_OnCreateLobbyStarted;
        RaceGameLobby.Instance.OnCreateLobbyFailed += RaceGameLobby_OnCreateLobbyFailed;
        RaceGameLobby.Instance.OnJoinStarted += RaceGameLobby_OnJoinStarted;
        RaceGameLobby.Instance.OnJoinFailed += RaceGameLobby_OnJoinFailed;
        RaceGameLobby.Instance.OnQuickJoinFailed += RaceGameLobby_OnQuickJoinFailed;

        Hide();
    }

    private void RaceGameLobby_OnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("¡No se encontró una sala a la cual entrar rápidamente!");
    }

    private void RaceGameLobby_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("¡Error al ingresar a la sala!");
    }

    private void RaceGameLobby_OnJoinStarted(object sender, EventArgs e)
    {
        ShowMessage("Entrando a la sala...");
    }

    private void RaceGameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("¡Error al crear la sala!");
    }

    private void RaceGameLobby_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        ShowMessage("Creando una sala...");
    }

    private void RaceMultiplayer_OnFailedToJoinGame(object sender, EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Error al ingresar a la sala");
        }
        else 
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        RaceMultiplayerController.Instance.OnFailedToJoinGame -= RaceMultiplayer_OnFailedToJoinGame;
        RaceGameLobby.Instance.OnCreateLobbyStarted -= RaceGameLobby_OnCreateLobbyStarted;
        RaceGameLobby.Instance.OnCreateLobbyFailed -= RaceGameLobby_OnCreateLobbyFailed;
        RaceGameLobby.Instance.OnJoinStarted -= RaceGameLobby_OnJoinStarted;
        RaceGameLobby.Instance.OnJoinFailed -= RaceGameLobby_OnJoinFailed;
        RaceGameLobby.Instance.OnQuickJoinFailed -= RaceGameLobby_OnQuickJoinFailed;
    }
}
