using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

namespace KartGame.KartSystems
{
    public class RaceController : NetworkBehaviour
    {


        private List<RaceQuestionModel> raceQuestions;
        private string correctAnswer;

        [SerializeField] private Transform playerPrefab;

        [SerializeField] private GameObject prefabIA;

        private static GameObject spawnPointParent;
        private NetworkList<Vector3> networkSpawnPositionList = new NetworkList<Vector3>();
        private List<Vector3> spawnPositionListSingleplayer = new List<Vector3>();


        [Space(10)]
        [Header("Quick Time Event")]
        [SerializeField] private GameObject canvasRaceQuestions;
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



        // Start is called before the first frame update
        void Start()
        {
            raceQuestions = RaceQuestionModel.GetRaceQuestions();
            raceQuestions.ForEach(question => Debug.Log(question.wording));
            //FillQuestionText();
            SelectNewQuestion();

            if (!RaceMultiplayerController.playMultiplayer)
            {
                InitializeSpawnPointsSingleplayer();
                Vector3 spawnPosition = GetRandomSpawnPoint();
                Transform playerTransform = Instantiate(playerPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));

                playerTransform.tag = "Player";
                playerTransform.name = "KartPlayer";
                ArcadeKart arcadeKart = playerTransform.GetComponent<ArcadeKart>();
                Destroy(arcadeKart);

                AutoAssignEvent autoAssignEvent = playerTransform.GetComponentInChildren<AutoAssignEvent>();
                autoAssignEvent.AssignKeyboardInput();
                AutoAssignEventStop autoAssignEventStop = playerTransform.GetComponentInChildren<AutoAssignEventStop>();
                autoAssignEventStop.AssignKeyboardInput();

                //Instantiate the IA pilots
                Vector3 spawnPositionIA = GetRandomSpawnPoint();
                Transform iaTransform = Instantiate(prefabIA.transform, spawnPositionIA, Quaternion.Euler(0, 180, 0));
                Debug.Log(iaTransform.name);
                iaTransform.gameObject.SetActive(true);

                //! CHANGE THE BOOLEAN VARIABLE 2 START THE IA MOVEMENT

                AIController controllerAI = iaTransform.GetComponent<AIController>();
                StartCoroutine(StartIAMovement(controllerAI));
                // controllerAI.shouldMove = true;
            }
        }

        private void FixedUpdate()
        {
            // Verifica si se ha tocado la pantalla
            if (feedbackIsActive == true && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // Desactiva el Canvas
                feedbackPanel.gameObject.SetActive(false);
                canvasRaceQuestions.SetActive(true);
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                InitializeSpawnPoints();
                NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
                Debug.Log("Im in server in race");
            }
        }
        IEnumerator StartIAMovement(AIController controllerAI)
        {
            yield return new WaitForSeconds(5);
            controllerAI.shouldMove = true;
        }
        private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                Vector3 spawnPosition = GetRandomSpawnPoint();
                Transform playerTransform = Instantiate(playerPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
                ArcadeKartSingleplayer arcadeKart = playerTransform.GetComponent<ArcadeKartSingleplayer>();
                Destroy(arcadeKart);
                playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            }
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
                {
                    NetworkObject networkObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
                    if (networkObject != null)
                    {
                        networkObject.Despawn();
                        Destroy(networkObject.gameObject);
                    }
                }

                Debug.Log($"Client {clientId} disconnected and resources cleaned up.");
            }
        }

        private void InitializeSpawnPoints()
        {
            if (IsServer)
            {
                spawnPointParent = GameObject.FindGameObjectWithTag("Spawnpoint");
                if (spawnPointParent != null)
                {
                    Debug.Log("I find spawnpoint");
                    foreach (Transform child in spawnPointParent.transform)
                    {
                        networkSpawnPositionList.Add(child.position);
                    }
                }
                else
                {
                    Debug.LogError("Spawnpoint parent not found.");
                }
            }
        }

        private void InitializeSpawnPointsSingleplayer()
        {
            if (RaceMultiplayerController.playMultiplayer == false)
            {
                spawnPointParent = GameObject.FindGameObjectWithTag("Spawnpoint");
                if (spawnPointParent != null)
                {
                    Debug.Log("I find spawnpoint");
                    foreach (Transform child in spawnPointParent.transform)
                    {
                        spawnPositionListSingleplayer.Add(child.position);
                    }
                }
                else
                {
                    Debug.LogError("Spawnpoint parent not found.");
                }
            }
        }

        public Vector3 GetRandomSpawnPoint()
        {
            if (RaceMultiplayerController.playMultiplayer)
            {
                if (networkSpawnPositionList.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, networkSpawnPositionList.Count);
                    Vector3 spawnPoint = networkSpawnPositionList[randomIndex];
                    networkSpawnPositionList.RemoveAt(randomIndex);
                    return spawnPoint;
                }
                else
                {
                    Debug.LogError("No spawn points available or spawnPositionList not initialized.");
                    return Vector3.zero;
                }
            }
            else
            {
                if (spawnPositionListSingleplayer.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, spawnPositionListSingleplayer.Count);
                    Vector3 spawnPoint = spawnPositionListSingleplayer[randomIndex];
                    spawnPositionListSingleplayer.RemoveAt(randomIndex);
                    return spawnPoint;
                }
                else
                {
                    Debug.LogError("No spawn points available or spawnPositionList not initialized.");
                    return Vector3.zero;
                }
            }

        }

        public void SelectNewQuestion()
        {
     
            RaceQuestionModel question = raceQuestions.First();
            if(question  == null)
            {
                Debug.Log("NO hay más preguntas madafakas");
                return;
            }

            Debug.Log("-------------------------------");
            Debug.Log($"Preguntas disponibles {raceQuestions.Count}");
            Debug.Log($"Respuesta {question.correct_option}");
            Debug.Log("-------------------------------");
            

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
            //int index = UnityEngine.Random.Range(0, raceQuestions.Count);
            //RaceQuestionModel raceQuestion = raceQuestions[index];
            //raceQuestions.RemoveAt(index);
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

            if (correctAnswer == TextAnswer) {
                CorrectBehaviour();
            }
            else { 
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

            // Change to next question.

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


        }

        public void ActivateRaceQuestionCanvas()
        {
            // Desactivate the image feedback
            negativeFeedBackImage.gameObject.SetActive(false);
            positiveFeedBackImage.gameObject.SetActive(false);

            questionPanel.gameObject.SetActive(true);
            canvasRaceQuestions.SetActive(true);
        }

        public void DeactivateRaceQuestionCanvas()
        {
            canvasRaceQuestions.SetActive(false);
        }
    }
}