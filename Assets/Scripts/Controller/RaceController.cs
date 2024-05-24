using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class RaceController : NetworkBehaviour
{
    [SerializeField] private GameObject canvasRaceQuestions;
    [SerializeField] private TextMeshProUGUI textQuestion;
    [SerializeField] private GameObject[] answersGameObjects;
    [SerializeField] private GameObject positiveAudio;
    [SerializeField] private GameObject negativeAudio;

    private List<RaceQuestionModel> raceQuestions;
    private string correctAnswer;

    [SerializeField] private Transform playerPrefab;

    private static GameObject spawnPointParent;
    private NetworkList<Vector3> networkSpawnPositionList = new NetworkList<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        raceQuestions = RaceQuestionModel.GetRaceQuestions();
        raceQuestions.ForEach(question => Debug.Log(question.wording));
        FillQuestionText();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InitializeSpawnPoints();
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Vector3 spawnPosition = GetRandomSpawnPoint();
            Transform playerTransform = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong obj)
    {
        throw new NotImplementedException();

        // Send the player to the menu
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

    public Vector3 GetRandomSpawnPoint()
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

    private void FillQuestionText()
    {
        int index = UnityEngine.Random.Range(0, raceQuestions.Count);
        RaceQuestionModel raceQuestion = raceQuestions[index];
        raceQuestions.RemoveAt(index);

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
        if (correctAnswer == TextAnswer) { CorrectBehaviour(); }
        else { IncorrectBehaviour(); }
    }

    private void CorrectBehaviour()
    {
        positiveAudio.GetComponent<AudioSource>().Play();
    }

    private void IncorrectBehaviour()
    {
        negativeAudio.GetComponent<AudioSource>().Play();
    }

    private void ActivateRaceQuestionCanvas()
    {
        canvasRaceQuestions.SetActive(true);
    }

    private void DeactivateRaceQuestionCanvas()
    {
        canvasRaceQuestions.SetActive(false);
    }
}
