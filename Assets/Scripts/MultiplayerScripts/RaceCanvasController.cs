using TMPro;
using UnityEngine;
using System.Collections;

public class RaceCanvasController : MonoBehaviour
{
    private TextMeshProUGUI countdownText;

    private void Start()
    {
        RaceMultiplayerController.Instance.OnStateChanged += RaceController_OnStateChanged;
        RaceMultiplayerController.Instance.OnLocalPlayerReadyChanged += RaceController_OnLocalPlayerReadyChanged;

        InitializeCountdownText();
        Hide();
    }

    private void Update()
    {
        if (RaceMultiplayerController.Instance.IsCountdownToStartActive())
        {
            countdownText.text = Mathf.CeilToInt(RaceMultiplayerController.Instance.countDownToStartTimer.Value).ToString();
        }
        else
        {
            Hide();
        }
    }

    private void InitializeCountdownText()
    {
        if (countdownText == null)
        {
            countdownText = GetComponentInChildren<TextMeshProUGUI>();
            if (countdownText == null)
            {
                Debug.LogError("TextMeshProUGUI component not found on the child object.");
            }
        }
    }

    private void RaceController_OnLocalPlayerReadyChanged(object sender, System.EventArgs e)
    {
        if (RaceMultiplayerController.Instance.IsLocalPlayerReady())
        {
            Hide();
        }
    }

    private void RaceController_OnStateChanged(object sender, System.EventArgs e)
    {
        if (RaceMultiplayerController.Instance.IsCountdownToStartActive())
        {
            Show();
        }
    }

    private void OnDestroy()
    {
        if (RaceMultiplayerController.Instance != null)
        {
            RaceMultiplayerController.Instance.OnStateChanged -= RaceController_OnStateChanged;
            RaceMultiplayerController.Instance.OnLocalPlayerReadyChanged -= RaceController_OnLocalPlayerReadyChanged;
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
