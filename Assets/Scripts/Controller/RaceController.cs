using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class RaceController : MonoBehaviour
{
    [SerializeField] private GameObject canvasRaceQuestions;
    [SerializeField] private TextMeshProUGUI textQuestion;
    [SerializeField] private GameObject[] answersGameObjects;
    [SerializeField] private GameObject positiveAudio;
    [SerializeField] private GameObject negativeAudio;

    private List<RaceQuestionModel> raceQuestions;
    private string correctAnswer;

    private static GameObject spawnPointParent;
    private static List<Transform> spawnPositionTransformList;

    // Start is called before the first frame update
    void Start()
    {
        raceQuestions = RaceQuestionModel.GetRaceQuestions();
        raceQuestions.ForEach(question => Debug.Log(question.wording));
        FillQuestionText();

        InitializeSpawnPoints();
    }

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
        int index = Random.Range(0, raceQuestions.Count);
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
            int j = Random.Range(0, i + 1);
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
