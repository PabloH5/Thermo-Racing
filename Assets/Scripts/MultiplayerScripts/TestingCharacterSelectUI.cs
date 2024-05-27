using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Services.Lobbies.Models;

public class TestingRaceSelectUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button readyButton;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;

    void Awake()
    {
        mainMenuButton.onClick.AddListener(() => {
            RaceGameLobby.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("MainMenu");
        });

        readyButton.onClick.AddListener(() => {
            SelectRaceReady.Instance.SetPlayerReady();
        });
    }

    void Start()
    {
        Lobby lobby = RaceGameLobby.Instance.GetLobby();

        lobbyNameText.text = lobby.Name;
        lobbyCodeText.text = lobby.LobbyCode;   
    }
}
