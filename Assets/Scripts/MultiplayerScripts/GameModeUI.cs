using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeUI : MonoBehaviour
{
    [SerializeField] private Button playSingleplayerButton;
    [SerializeField] private Button playMultiplayerButton;

    void Awake()
    {
        playSingleplayerButton.onClick.AddListener(() => {
            RaceMultiplayerController.playMultiplayer = false;
            SceneManager.LoadScene("SelectRaceSingleplayer");
        });
        playMultiplayerButton.onClick.AddListener(() => {
            RaceMultiplayerController.playMultiplayer = true;
            SceneManager.LoadScene("LobbyScene");
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
