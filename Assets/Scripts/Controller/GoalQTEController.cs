using KartGame.KartSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalQTEController : MonoBehaviour
{
    private CheckList checkpointManager;
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

    [Space(10)]
    [Header("Quick Time Event")]
    private List<RaceQuestionModel> raceQuestions;
    private string correctAnswer;
    public Boolean LastQuestion { get; set; }

    [SerializeField] private TextMeshProUGUI textQuestion;
    [SerializeField] private GameObject[] answersGameObjects;
    [SerializeField] private GameObject positiveAudio;
    [SerializeField] private GameObject negativeAudio;
    
    [Space(5)]
    [Header("Feedback UI")]
    [SerializeField] private bool feedbackIsActive;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TMP_Text correctAnswerText;
    [SerializeField] private TMP_Text feedbackTitle;
    [SerializeField] private Image positiveFeedBackImage;
    [SerializeField] private Image negativeFeedBackImage;


    // Only for debug quickly
    List<GameObject> objectList = new List<GameObject>();



    void Start()
    {
        raceQuestions = RaceQuestionModel.GetRaceQuestions();
        raceQuestions.ForEach(question => Debug.Log(question.wording));
    
        arcadeKartScriptManager = GetComponentInParent<ArcadeKart>();
        if (arcadeKartScriptManager == null) { arcadeKartSingleplayerScriptManager = GetComponentInParent<ArcadeKartSingleplayer>(); }

        checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckList>();

        objectListSpawnpoints = checkpointManager.GetObjectList();
        objectList = checkpointManager.GetObjectList();
    }

    private void FixedUpdate()
    {
        // Verifica si se ha tocado la pantalla
        if (feedbackIsActive == true && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Desactiva el Canvas
            feedbackPanel.gameObject.SetActive(false);
        }
    }

    #region 

        public void SelectNewQuestion()
        {

            RaceQuestionModel question = raceQuestions.First();
            if (question == null)
            {
                return;
            }

            // Debug.Log("-------------------------------");
            // Debug.Log($"Preguntas disponibles {raceQuestions.Count}");
            // Debug.Log($"Respuesta {question.correct_option}");
            // Debug.Log("-------------------------------");

            string[] options = new string[]
            {
                    question.first_option,
                    question.second_option,
                    question.third_option,
                    question.fourth_option
            };

            for (int i = options.Length - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                string temp = options[i];
                options[i] = options[j];
                options[j] = temp;
            }

            textQuestion.text = question.wording;

            for (int i = 0; i < answersGameObjects.Length && i < options.Length; i++)
            {
                TextMeshProUGUI answerText = answersGameObjects[i].GetComponentInChildren<TextMeshProUGUI>();
                if (answerText != null)
                {
                    answerText.text = options[i];
                }
            }

            correctAnswer = question.correct_option;

            raceQuestions.RemoveAt(0);
        }

        private void FillQuestionText()
        {
            RaceQuestionModel raceQuestion = raceQuestions.First();
            raceQuestions.RemoveAt(0);

            string[] options = new string[]
            {
                raceQuestion.first_option,
                raceQuestion.second_option,
                raceQuestion.third_option,
                raceQuestion.fourth_option
            };

            for (int i = options.Length - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                string temp = options[i];
                options[i] = options[j];
                options[j] = temp;
            }

            textQuestion.text = raceQuestion.wording;

            for (int i = 0; i < answersGameObjects.Length && i < options.Length; i++)
            {
                TextMeshProUGUI answerText = answersGameObjects[i].GetComponentInChildren<TextMeshProUGUI>();
                if (answerText != null)
                {
                    answerText.text = options[i];
                }
            }

            correctAnswer = raceQuestion.correct_option;
        }

        public void ValidateCorrectAnswer(int NumberOfAnswer)
        {
            string TextAnswer = answersGameObjects[NumberOfAnswer].GetComponentInChildren<TextMeshProUGUI>().text;

            if (correctAnswer == TextAnswer)
            {
                CorrectBehaviour();
            }
            else
            {
                IncorrectBehaviour();
            }
        }

        private void CorrectBehaviour()
        {
            // Desactive the question panel
            questionPanel.gameObject.SetActive(false);

            // Show positive feedback;
            feedbackIsActive = true;
            positiveAudio.GetComponent<AudioSource>().Play();
            feedbackTitle.text = "¡Has acertado!";
            feedbackPanel.gameObject.SetActive(true);

            // Active the positive feedback image.
            positiveFeedBackImage.gameObject.SetActive(true);
            correctAnswerText.text = correctAnswer;

            if (LastQuestion == true)
            {
                FinishRace();
            }
            else
            {
                // Allow user movement.
                FreezePlayer(false);
            }
        }

        private void IncorrectBehaviour()
        {
            // ---
            // Desactive the question panel
            questionPanel.gameObject.SetActive(false);

            // Show negative feedback;
            feedbackIsActive = true;
            negativeAudio.GetComponent<AudioSource>().Play();
            feedbackTitle.text = "¡Has fallado!";
            feedbackPanel.gameObject.SetActive(true);


            // Active the negative feedback image.
            negativeFeedBackImage.gameObject.SetActive(true);
            correctAnswerText.text = correctAnswer;

            if (LastQuestion == true)
            {
                FinishRace();
            }
            else
            {
                // Allow user movement.
                FreezePlayer(false);
            }

        }

        public void ActivateRaceQuestionCanvas()
        {
            //quickTimeEventController.ModifyUserConstraints(1);


            // Desactivate the image feedback
            negativeFeedBackImage.gameObject.SetActive(false);
            positiveFeedBackImage.gameObject.SetActive(false);

            questionPanel.gameObject.SetActive(true);
        }

    #endregion

    public void ManageLapBehaviour(GameObject other)
    {
        if (other.gameObject == this.gameObject)
        {
            NetworkObject networkObject = GetComponentInParent<NetworkObject>();

            if (RaceMultiplayerController.playMultiplayer && (networkObject == null || !networkObject.IsOwner))
            {
                Debug.Log("I am not the owner");
                return;
            }

            arcadeKartScriptManager = GetComponentInParent<ArcadeKart>();
            if (arcadeKartScriptManager == null) { arcadeKartSingleplayerScriptManager = GetComponentInParent<ArcadeKartSingleplayer>(); }

            /// For debug only
                // if (arcadeKartScriptManager != null) 
                // {
                //     foreach (GameObject checkpoint in objectList)
                //     {
                //         arcadeKartScriptManager.AddOrUpdateCheckpoint(checkpoint.name, true, DateTime.Now); 
                //     }
                // }
                // if (arcadeKartSingleplayerScriptManager != null)
                // {
                //     foreach (GameObject checkpoint in objectList)
                //     {
                //         arcadeKartSingleplayerScriptManager.AddOrUpdateCheckpoint(checkpoint.name, true, DateTime.Now); 
                //     }
                // }
            ///

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
                        return;
                    }
                }
                if (arcadeKartSingleplayerScriptManager != null)
                {
                    if(arcadeKartSingleplayerScriptManager._CanGoForLap == false)
                    {
                        return;
                    }
                }
            }

            FreezePlayer(true);

            // 2. If the user will start the second lap, launch the Quick Time Event.
            if (user.numberOfLaps == 1)
            {
                user.numberOfLaps++;
                // 2.1 Select the new question.
                SelectNewQuestion();
                ActivateRaceQuestionCanvas();
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
                } else if (arcadeKartSingleplayerScriptManager != null) {
                    foreach (GameObject checkpoint in objectListSpawnpoints)
                    {
                        arcadeKartSingleplayerScriptManager.AddOrUpdateCheckpoint(checkpoint.name, false, DateTime.Now);
                    }
                    arcadeKartSingleplayerScriptManager.AddOrUpdateCheckpoint("Goal line", true, DateTime.Now);
                    Debug.Log("I do it - Lap 1 - Singleplayer");
                }

                return;
            }

            if (user.numberOfLaps == 2)
            {
                user.numberOfLaps++;
                // 2.1 Select the new question.
                SelectNewQuestion();
                ActivateRaceQuestionCanvas();

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
                } else if (arcadeKartSingleplayerScriptManager != null) {
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
                LastQuestion = true;
                SelectNewQuestion();
                ActivateRaceQuestionCanvas();

                if (arcadeKartScriptManager != null) 
                {
                    foreach (GameObject checkpoint in objectListSpawnpoints)
                    {
                        arcadeKartScriptManager.AddOrUpdateCheckpoint(checkpoint.name, false, DateTime.Now);
                    }
                    arcadeKartScriptManager.AddOrUpdateCheckpoint("Goal line", true, DateTime.Now);
                } else if (arcadeKartSingleplayerScriptManager != null) {
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
        StartCoroutine(Nose());
    }

    public IEnumerator Nose()
    {
        winnerInterface.SetActive(true);
        yield return new WaitForSeconds(2);
        leaderboardInterface.SetActive(true);
        Button backToMenuButton = leaderboardInterface.GetComponentInChildren<Button>();
        backToMenuButton.onClick.AddListener(() => {
            SceneManager.LoadScene("MainMenu");
        });
    }
}
