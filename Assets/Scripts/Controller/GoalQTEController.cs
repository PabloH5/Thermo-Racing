using KartGame.KartSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GoalQTEController : MonoBehaviour
{
    private CheckList checkpointManager;
    private RaceController raceController;
    private List<GameObject> objectListSpawnpoints = new List<GameObject>();

    [Space(10)]
    [Header("Laps UI")]
    [SerializeField] private List<Sprite> spriteLaps;
    [SerializeField] private Image currentSpriteLap;
    [SerializeField] private TMP_Text currentLapNumber;


    [Space(10)]
    [Header("Finish the race UI")]
    [SerializeField] private GameObject winnerInterface;
    [SerializeField] private GameObject leaderboardInterface;

    private KeyboardInput userKeyBoard;
    private ArcadeKart arcadeKartScriptManager;
    private ArcadeKartSingleplayer arcadeKartSingleplayerScriptManager;

    void Start()
    {
        arcadeKartScriptManager = GetComponentInParent<ArcadeKart>();
        if (arcadeKartScriptManager == null) { arcadeKartSingleplayerScriptManager = GetComponentInParent<ArcadeKartSingleplayer>(); }

        checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckList>();
        raceController = GameObject.FindGameObjectWithTag("RaceManager").GetComponent<RaceController>();

        raceController.SetQTEcontroller(this);
        objectListSpawnpoints = checkpointManager.GetObjectList();
    }

    public void ManageLapBehaviour(GameObject other)
    {
        if (other.gameObject == this.gameObject)
        {
            Debug.Log("USUARIO Piso la meta");
            userKeyBoard = other.GetComponentInParent<KeyboardInput>();
            if (userKeyBoard == null)
            {
                Debug.LogError("KeyboardInput not found on the parent of the object");
                return;
            }

            UserRaceInformation user = other.GetComponentInParent<UserRaceInformation>();
            if (user == null)
            {
                Debug.LogError("UserRaceInformation not found on the parent of the object");
                return;
            }

            if (user.numberOfLaps == 0)
            {
                user.numberOfLaps++;
                Debug.Log("TOQUE LA META PRIMERA VEZ");
                Debug.Log(user.numberOfLaps);
                return;
            }

            Debug.Log(other.name);
            Debug.Log(user.numberOfLaps);

            // Check if the player can proceed to the next lap
            if (user.numberOfLaps > 0)
            {
                if (arcadeKartScriptManager != null) 
                {
                    if(arcadeKartScriptManager._CanGoForLap == false)
                    {
                        Debug.Log("El jugador multiplayer no puede avanzar a la siguiente vuelta.");
                        return;
                    }
                }
                if (arcadeKartSingleplayerScriptManager != null)
                {
                    if(arcadeKartSingleplayerScriptManager._CanGoForLap == false)
                    {
                        Debug.Log("El jugador singleplayer no puede avanzar a la siguiente vuelta.");
                        return;
                    }
                }
            }

            FreezePlayer(true);

            // 2. If the user will start the second lap, launch the Quick Time Event.
            if (user.numberOfLaps == 1)
            {
                Debug.Log("TOQUE LA META PRIMERA VUELTA");
                user.numberOfLaps++;
                // 2.1 Select the new question.
                raceController.ActivateRaceQuestionCanvas();
                // 3 Block the user movement

                // 3.3 Until the user response.

                // 3.4 Give user buff 

                // 6. Increment the number of laps.

                // Put the current lap in UI.
                currentLapNumber.text = $"{user.numberOfLaps}";
                currentSpriteLap.sprite = spriteLaps[1];

                if (arcadeKartScriptManager != null) 
                {
                    foreach (GameObject checkpoint in objectListSpawnpoints)
                    {
                        arcadeKartScriptManager.AddOrUpdateCheckpoint(checkpoint.name, false, DateTime.Now);
                    }
                    arcadeKartScriptManager.AddOrUpdateCheckpoint("Goal line", true, DateTime.Now);
                } else {
                    foreach (GameObject checkpoint in objectListSpawnpoints)
                    {
                        arcadeKartSingleplayerScriptManager.AddOrUpdateCheckpoint(checkpoint.name, false, DateTime.Now);
                    }
                    arcadeKartSingleplayerScriptManager.AddOrUpdateCheckpoint("Goal line", true, DateTime.Now);
                }

                return;
            }

            if (user.numberOfLaps == 2)
            {
                user.numberOfLaps++;
                // 2.1 Select the new question.
                raceController.SelectNewQuestion();
                raceController.ActivateRaceQuestionCanvas();

                // Put the current lap in UI.
                currentLapNumber.text = $"{user.numberOfLaps}";
                currentSpriteLap.sprite = spriteLaps[2];

                if (arcadeKartScriptManager != null) 
                {
                    foreach (GameObject checkpoint in objectListSpawnpoints)
                    {
                        arcadeKartScriptManager.AddOrUpdateCheckpoint(checkpoint.name, false, DateTime.Now);
                    }
                    arcadeKartScriptManager.AddOrUpdateCheckpoint("Goal line", true, DateTime.Now);
                } else {
                    foreach (GameObject checkpoint in objectListSpawnpoints)
                    {
                        arcadeKartSingleplayerScriptManager.AddOrUpdateCheckpoint(checkpoint.name, false, DateTime.Now);
                    }
                    arcadeKartSingleplayerScriptManager.AddOrUpdateCheckpoint("Goal line", true, DateTime.Now);
                }

                return;
            }

            // The final question
            if (user.numberOfLaps == 3)
            {
                user.numberOfLaps++;
                raceController.LastQuestion = true;
                raceController.SelectNewQuestion();
                raceController.ActivateRaceQuestionCanvas();

                if (arcadeKartScriptManager != null) 
                {
                    foreach (GameObject checkpoint in objectListSpawnpoints)
                    {
                        arcadeKartScriptManager.AddOrUpdateCheckpoint(checkpoint.name, false, DateTime.Now);
                    }
                    arcadeKartScriptManager.AddOrUpdateCheckpoint("Goal line", true, DateTime.Now);
                } else {
                    foreach (GameObject checkpoint in objectListSpawnpoints)
                    {
                        arcadeKartSingleplayerScriptManager.AddOrUpdateCheckpoint(checkpoint.name, false, DateTime.Now);
                    }
                    arcadeKartSingleplayerScriptManager.AddOrUpdateCheckpoint("Goal line", true, DateTime.Now);
                }

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
        yield return new WaitForSeconds(2);
        leaderboardInterface.SetActive(true);
    }
}
