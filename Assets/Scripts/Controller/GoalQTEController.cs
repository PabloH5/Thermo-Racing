using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalQTEController : MonoBehaviour
{
    public CheckList CheckpointManager;
    [SerializeField] private RaceController raceController;
    //private Rigidbody userRigidbody;
    private KeyboardInput userKeyBoard;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //userRigidbody = other.GetComponentInParent<Rigidbody>();
            userKeyBoard = other.GetComponentInParent<KeyboardInput>();
            Debug.Log("USUARIO Piso la meta");
            Debug.Log(other.name);
            UserRaceInformation user = other.GetComponentInParent<UserRaceInformation>();

            CheckpointManager.ToggleObjects();
            Debug.Log(user.numberOfLaps);


            // 1. If the user is in the first lap, do not anything.
            if (user.numberOfLaps == 0)
            {
                user.numberOfLaps++;
                return;
            }

            FreezePlayer(true);
            //ModifyUserConstraints(1);

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

            if (user.numberOfLaps >= 2)
            {
                // 2.1 Select the new question.
                raceController.SelectNewQuestion();
                raceController.ActivateRaceQuestionCanvas();
                user.numberOfLaps++;
                return;
            }
        }


    }

    //public void ModifyUserConstraints(int variableToModify)
    //{
    //    if(variableToModify == 0) {
    //        userRigidbody.constraints = RigidbodyConstraints.None;
    //        userRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
    //    }
    //    else
    //    {
    //        userRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    //    }
    //}

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
}
