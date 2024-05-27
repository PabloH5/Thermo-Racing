using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestingCharacterSelectUISingleplayer : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button raceTrack1PlayButton;
    [SerializeField] private Button raceTrack2PlayButton;
    [SerializeField] private Button readyToPlayButton;

    private string actualRace = "";

    void Awake()
    {
        mainMenuButton.onClick.AddListener(() => {
            SceneManager.LoadScene("ChooseRaceGameMode");
        });

        raceTrack1PlayButton.onClick.AddListener(() => {
            ChooseGameRace("Track1");
        });

        raceTrack2PlayButton.onClick.AddListener(() => {
            ChooseGameRace("Track2");
        });

        readyToPlayButton.onClick.AddListener(() => {
            Debug.Log("Changing Scene...");
            SceneManager.LoadScene(GetActualRace());
        });
    }

    private void ChooseGameRace(string choosenRace)
    {
        actualRace = choosenRace;
    }

    private string GetActualRace()
    {
        if (actualRace == "" || actualRace == "Track1")
        {
            return "Track1";
        } else if (actualRace == "Track2")
        {
            return "Track2";
        }
        return default;
    }

}
