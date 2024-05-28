using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalQTEController : MonoBehaviour
{
    public CheckList CheckpointManager;
    [SerializeField] private RaceController raceController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("USUARIO Piso la meta - LANZAR QUICK TIME EVENT.");
            UserRaceInformation user = other.GetComponentInParent<UserRaceInformation>();
            CheckpointManager.ToggleObjects();
            Debug.Log(user.numberOfLaps);
            

            // 1. If the user is in the first lap, do not anything.
            if (user.numberOfLaps == 0)
            {
                user.numberOfLaps++;
                return;
            }

            if (user.numberOfLaps == 4)
            {
                Debug.Log("GG");
                return;
            }


            // 2. If the user will start the second lap, launch the Quick Time Event.
            if (user.numberOfLaps == 1)
            {
                // 2.1 Select the new question.
                raceController.ActivateRaceQuestionCanvas();
                // 3 Block the user movement

                // 3.3 Until the user response.

                // 3.4 Give user buff 


                // 6. Increment the number of laps.
                user.numberOfLaps++;
                return;


            }

            if(user.numberOfLaps >= 2)
            {
                // 2.1 Select the new question.
                raceController.SelectNewQuestion();
                raceController.ActivateRaceQuestionCanvas();
                user.numberOfLaps++;
                return;
            }
        }
        
    }
}
