using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionResponseMessageUI : MonoBehaviour
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

        Hide();
    }

    private void RaceMultiplayer_OnFailedToJoinGame(object sender, EventArgs e)
    {
        Show();

        messageText.text = NetworkManager.Singleton.DisconnectReason;

        if (messageText.text == "")
        {
            messageText.text = "No se pudo conectar a la sala";
        }
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
    }
}
