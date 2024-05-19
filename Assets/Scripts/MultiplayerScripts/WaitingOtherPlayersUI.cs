using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingOtherPlayersUI : MonoBehaviour
{
    private void Start()
    {
        RaceMultiplayerController.Instance.OnLocalPlayerReadyChanged += RaceController_OnLocalPlayerReadyChanged;
        RaceMultiplayerController.Instance.OnStateChanged += RaceController_OnStateChanged;

        Hide();
    }

    private void RaceController_OnStateChanged(object sender, System.EventArgs e)
    {
        if (RaceMultiplayerController.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void RaceController_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        if (RaceMultiplayerController.Instance.IsLocalPlayerReady())
        {
            Show();
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
}
