using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    [SerializeField] private GameObject canvasRaceQuestions;
    [SerializeField] private TextMeshProUGUI textQuestion;  
    [SerializeField] private GameObject[] answersGameObjects;
    [SerializeField] private GameObject positiveAudio;
    [SerializeField] private GameObject negativeAudio;

    private List<RaceQuestionModel> raceQuestions;
    private string correctAnswer;

    // Start is called before the first frame update
    void Start()
    { 
        raceQuestions = RaceQuestionModel.GetRaceQuestions();
        raceQuestions.ForEach(question => Debug.Log(question.wording));
        FillQuestionText();
    }

    private void FillQuestionText()
    {
        // Step 1: Select the question
        int index = Random.Range(0, raceQuestions.Count);
        RaceQuestionModel raceQuestion = raceQuestions[index];
        raceQuestions.RemoveAt(index);

        // Step 2: Prepare options
        string[] options = new string[]
        {
            raceQuestion.first_option,
            raceQuestion.second_option,
            raceQuestion.third_option,
            raceQuestion.fourth_option
        };

        // Step 3: Shuffle options
        for (int i = options.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = options[i];
            options[i] = options[j];
            options[j] = temp;
        }

        // Set the question text
        textQuestion.text = raceQuestion.wording;

        // Step 4: Assign to answer objects
        for (int i = 0; i < answersGameObjects.Length && i < options.Length; i++)
        {
            TextMeshProUGUI answerText = answersGameObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            if (answerText != null)
            {
                answerText.text = options[i];
            }
        }

        // Step 5: Save the correct answer
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
