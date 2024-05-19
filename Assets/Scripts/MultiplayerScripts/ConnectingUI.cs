using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        RaceMultiplayerController.Instance.OnTryingToJoinGame += RaceMultiplayer_OnTryingToJoinGame;
        RaceMultiplayerController.Instance.OnFailedToJoinGame += RaceMultiplayer_OnFailedToJoinGame;
        Hide();
    }

    private void RaceMultiplayer_OnFailedToJoinGame(object sender, EventArgs e)
    {
        Hide();
    }

    private void RaceMultiplayer_OnTryingToJoinGame(object sender, EventArgs e)
    {
        Show();
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
        RaceMultiplayerController.Instance.OnTryingToJoinGame -= RaceMultiplayer_OnTryingToJoinGame;
        RaceMultiplayerController.Instance.OnFailedToJoinGame -= RaceMultiplayer_OnFailedToJoinGame;
    }
}
