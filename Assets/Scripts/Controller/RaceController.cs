using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class RaceController : NetworkBehaviour
{
    public static RaceController Instance { get; private set; }

    [SerializeField] private GameObject canvasRaceQuestions;
    [SerializeField] private TextMeshProUGUI textQuestion;
    [SerializeField] private GameObject[] answersGameObjects;
    [SerializeField] private GameObject positiveAudio;
    [SerializeField] private GameObject negativeAudio;

    private List<RaceQuestionModel> raceQuestions;
    private string correctAnswer;

    private float countDownToStartTimer = 3.0f;

    private static GameObject spawnPointParent;
    private static List<Transform> spawnPositionTransformList;

    public enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);

    private bool _IsLocalPlayerReady { get; set; } = false;
    public event EventHandler OnLocalPlayerReadyChanged;
    public event EventHandler OnStateChanged;

    public class StateEventArgs : EventArgs
    {
        public State CurrentState { get; private set; }

        public StateEventArgs(State state)
        {
            CurrentState = state;
        }
    }

    private Dictionary<ulong, bool> playerReadyDictionary;


    private void Awake()
    {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        raceQuestions = RaceQuestionModel.GetRaceQuestions();
        raceQuestions.ForEach(question => Debug.Log(question.wording));
        FillQuestionText();

        InitializeSpawnPoints(); 
    }

    public void Update()
    {
        if (!IsServer)
        {
            return;
        }

        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if ( countDownToStartTimer < 0f)
                {
                    state.Value = State.GamePlaying;
    
                }
                break;
            case State.GamePlaying:
                break;
            case State.GameOver:
                break;
        }
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetPlayer()
    {
        SetPlayerReadyServerRpc();
    }

    public void VerifyIfPlayersReadyOnSpawn()
    {
        VerifyPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void VerifyPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                // This player is not ready
                allClientsReady = false;
                break;
            }
        }
        Debug.Log("AllClientsReady: " + allClientsReady);

        if(allClientsReady)
        {
            state.Value = State.CountdownToStart;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log($"Setting player ready for client {serverRpcParams.Receive.SenderClientId}");
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
    }

    public void LocalPlayerIsReady()
    {
        if (state.Value == State.WaitingToStart)
        {
            _IsLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsLocalPlayerReady()
    { return _IsLocalPlayerReady; }
    public bool IsGamePlaying()
    { return state.Value == State.GamePlaying; }
    public bool IsCountdownToStartActive()
    { return state.Value == State.CountdownToStart; }
        public bool IsGameOver()
    { return state.Value == State.GameOver; }

    private void InitializeSpawnPoints()
    {
        if (spawnPointParent == null)
        {
            Debug.Log("I find spawnpoint");
            spawnPointParent = GameObject.FindGameObjectWithTag("Spawnpoint");
            spawnPositionTransformList = new List<Transform>();
            foreach (Transform child in spawnPointParent.transform)
            {
                spawnPositionTransformList.Add(child);
            }
        }
        else
        {
            Debug.Log("I not found spawnpoint");
        }
    }

    public Vector3 GetRandomSpawnPoint()
    {
        if (spawnPositionTransformList != null && spawnPositionTransformList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, spawnPositionTransformList.Count);
            Transform spawnPoint = spawnPositionTransformList[randomIndex];
            spawnPositionTransformList.RemoveAt(randomIndex);
            return spawnPoint.position;
        }
        else
        {
            Debug.LogError("No spawn points available or spawnPositionTransformList not initialized.");
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
