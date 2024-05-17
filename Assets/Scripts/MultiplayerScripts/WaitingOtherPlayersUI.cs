using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingOtherPlayersUI : MonoBehaviour
{
    private void Start()
    {
        RaceController.Instance.OnLocalPlayerReadyChanged += RaceController_OnLocalPlayerReadyChanged;
        RaceController.Instance.OnStateChanged += RaceController_OnStateChanged;

        Hide();
    }

    private void RaceController_OnStateChanged(object sender, System.EventArgs e)
    {
        if (RaceController.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void RaceController_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        if (RaceController.Instance.IsLocalPlayerReady())
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
