using KartGame.KartSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalQTEController : MonoBehaviour
{
    [SerializeField] private CheckList CheckpointManager;
    [SerializeField] private RaceController raceController;

    [Space(10)]
    [Header("Laps UI")]
    [SerializeField] private List<Sprite> spriteLaps;
    [SerializeField] private Image currentSpriteLap;
    [SerializeField] private TMP_Text currentLapNumber;
    [SerializeField] private TMP_Text currentPosition;


    [Space(10)]
    [Header("Finish the race UI")]
    [SerializeField] private GameObject winnerInterface;
    [SerializeField] private GameObject leaderboardInterface;

    private KeyboardInput userKeyBoard;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            userKeyBoard = other.GetComponentInParent<KeyboardInput>();
            Debug.Log("USUARIO Piso la meta");
            UserRaceInformation user = other.GetComponentInParent<UserRaceInformation>();

            CheckpointManager.ToggleObjects();
            Debug.Log(other.name);
            Debug.Log(user.numberOfLaps);


            // 1. If the user touch first time the goa line
            if (user.numberOfLaps == -1)
            {
                Debug.Log("TOQUE LA META PRIMERA VEZ");
                user.numberOfLaps++;
                return;
            }

            FreezePlayer(true);

            user.numberOfLaps++;
            // 2. If the user will start the second lap, launch the Quick Time Event.
            if (user.numberOfLaps == 1)
            {
                Debug.Log("TOQUE LA META PRIMERA VUELTA");
                // 2.1 Select the new question.
                raceController.ActivateRaceQuestionCanvas();
                // 3 Block the user movement


                // 3.3 Until the user response.

                // 3.4 Give user buff 


                // 6. Increment the number of laps.

                // Put the current lap in UI.
                user.numberOfLaps++;
                currentLapNumber.text = $"{user.numberOfLaps}";
                currentSpriteLap.sprite = spriteLaps[1];
                return;
            }

            if (user.numberOfLaps == 3)
            {

                // 2.1 Select the new question.
                raceController.SelectNewQuestion();
                raceController.ActivateRaceQuestionCanvas();

                // Put the current lap in UI.
                currentLapNumber.text = $"{user.numberOfLaps}";
                currentSpriteLap.sprite = spriteLaps[2];
                return;
            }

            // The final question
            if (user.numberOfLaps == 4)
            {
                raceController.LastQuestion = true;
                raceController.SelectNewQuestion();
                raceController.ActivateRaceQuestionCanvas();
                return;
            }
        }
    }

    public void FreezePlayer(bool isFreezed)
    {
        if (isFreezed == true)
        {
            userKeyBoard.isInQuestion = true;
        }
        else
        {
            userKeyBoard.isInQuestion = false;
        }
    }

    public void FinishRace()
    {
        Debug.Log("ME INVOCARON SOG");

        StartCoroutine(Nose());

        
    }

    


    public IEnumerator Nose()
    {
        winnerInterface.SetActive(true);
        yield return new WaitForSeconds(5);
        leaderboardInterface.SetActive(true);
    }
}
