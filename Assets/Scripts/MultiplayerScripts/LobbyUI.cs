using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private LobbyCreateUI lobbyCreateUI;
    [SerializeField] private Button joinCodeButton;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;


    void Awake()
    {
        mainMenuButton.onClick.AddListener(() => {
            RaceGameLobby.Instance.LeaveLobby();
            SceneManager.LoadScene("MainMenu");
        });

        createLobbyButton.onClick.AddListener(() => {
            lobbyCreateUI.Show();
        });

        quickJoinButton.onClick.AddListener(() => {
            RaceGameLobby.Instance.QuickJoin();
        });

        joinCodeButton.onClick.AddListener(() => {
            RaceGameLobby.Instance.JoinWithCode(joinCodeInputField.text);
        });

        lobbyTemplate.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerNameText.text = RaceMultiplayerController.Instance.GetPlayerName();

        RaceGameLobby.Instance.OnLobbyListChanged += RaceGameLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void RaceGameLobby_OnLobbyListChanged(object sender, RaceGameLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach(Transform child in lobbyContainer)
        {
            if (child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }

    void OnDestroy()
    {
        RaceGameLobby.Instance.OnLobbyListChanged -= RaceGameLobby_OnLobbyListChanged;
    }
}
