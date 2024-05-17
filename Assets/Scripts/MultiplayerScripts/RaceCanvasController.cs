using TMPro;
using UnityEngine;
using System.Collections;

public class RaceCanvasController : MonoBehaviour
{
    private TextMeshProUGUI countdownText;

    private void Start()
    {
        RaceController.Instance.OnStateChanged += RaceController_OnStateChanged;
        RaceController.Instance.OnLocalPlayerReadyChanged += RaceController_OnLocalPlayerReadyChanged;

        InitializeCountdownText();
        Hide();
    }

    private IEnumerator StartCountdown()
    {
        for (int i = 3; i >= 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        Hide();
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
        if (RaceController.Instance.IsLocalPlayerReady())
        {
            Hide();
        }
    }

    private void RaceController_OnStateChanged(object sender, System.EventArgs e)
    {
        if (RaceController.Instance.IsCountdownToStartActive())
        {
            Show();
            StartCoroutine(StartCountdown());
        }
    }

    private void OnDestroy()
    {
        if (RaceController.Instance != null)
        {
            RaceController.Instance.OnStateChanged -= RaceController_OnStateChanged;
            RaceController.Instance.OnLocalPlayerReadyChanged -= RaceController_OnLocalPlayerReadyChanged;
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
