using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestingLobbyUI : MonoBehaviour
{

    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameButton;

    private void Awake()
    {
        createGameButton.onClick.AddListener(() => {
            RaceMultiplayerController.Instance.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("SelectRace", LoadSceneMode.Single);
        });

        joinGameButton.onClick.AddListener(() => {
            RaceMultiplayerController.Instance.StartClient();
        });
    }

}
