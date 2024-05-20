using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ThermoEcuacionadorController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Option1;
    [SerializeField] private TextMeshProUGUI Option2;
    [SerializeField] private TextMeshProUGUI Option3;
    [SerializeField] private TextMeshProUGUI Option4;
    [SerializeField] private THEController controller;
    public List<DragAndDropOP> dragAndDropOP;

    // Start is called before the first frame update
    void Start()
    {
        dragAndDropOP.Add(Option1.GetComponent<DragAndDropOP>());
        dragAndDropOP.Add(Option2.GetComponent<DragAndDropOP>());
        dragAndDropOP.Add(Option3.GetComponent<DragAndDropOP>());
        dragAndDropOP.Add(Option4.GetComponent<DragAndDropOP>());
        // TEQuestionModel question = TEQuestionModel.GetWheelById(controller.CurrentSentence);
        // SetUpTEQuestion(question);
    }
    //public void SetupTESentence(int sentenceNumber)
    //{
    //    TEQuestionModel question = TEQuestionModel.GetTEQuestionById(sentenceNumber);
    //    SetUpTEQuestion(question);
    //}

    public List<KeyValuePair<string, string>> ShuffleDicionaryOptions(Dictionary<string, string> possibleOptions)
    {
        // Create a instance to generate radom numbers.
        System.Random random = new System.Random();
        // Convert the dictionary into List to access to the elements through the index.
        List<KeyValuePair<string, string>> possibleOptionsList = possibleOptions.ToList();

        for (int i = possibleOptionsList.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);

            // Exchange values using a temporal variable.
            var temp = possibleOptionsList[i];
            possibleOptionsList[i] = possibleOptionsList[j];
            possibleOptionsList[j] = temp;
        }
        return possibleOptionsList;
    }

    public void AssignOptionsToGUI(List<KeyValuePair<string, string>> possibleOptionsList)
    {
        Option1.text = possibleOptionsList[0].Key;
        Option1.tag = possibleOptionsList[0].Value;
        Option2.text = possibleOptionsList[1].Key;
        Option2.tag = possibleOptionsList[1].Value;
        Option3.text = possibleOptionsList[2].Key;
        Option3.tag = possibleOptionsList[2].Value;
        Option4.text = possibleOptionsList[3].Key;
        Option4.tag = possibleOptionsList[3].Value;
    }

    public void SetUpTEQuestion(TEQuestionModel question)
    {
        if (question.first_option != null && question.second_option != null && question.third_option == null && question.fourth_option == null)
        {
            // There are two correct options in this question.
            Dictionary<string, string> possibleOptions = new Dictionary<string, string>(){
                {question.first_option, "CorrectOption1"},
                {question.second_option,"CorrectOption2" },
                {question.first_missing_word,"BadOption" },
                {question.second_missing_word,"BadOption"  }
            };

            List<KeyValuePair<string, string>> possibleOptionsList = ShuffleDicionaryOptions(possibleOptions);
            AssignOptionsToGUI(possibleOptionsList);
        }
        else if (question.third_option != null && question.fourth_option == null)
        {
            // There are three correct options in this question.
            Dictionary<string, string> possibleOptions = new Dictionary<string, string>(){
                {question.first_option, "CorrectOption1"},
                {question.second_option,"CorrectOption2" },
                {question.third_option,"CorrectOption3" },
                {question.first_missing_word,"BadOption"  }
            };

            List<KeyValuePair<string, string>> possibleOptionsList = ShuffleDicionaryOptions(possibleOptions);
            AssignOptionsToGUI(possibleOptionsList);
        }
        else if (question.fourth_option != null)
        {
            // There are four correct options in this question.
            Dictionary<string, string> possibleOptions = new Dictionary<string, string>(){
                {question.first_option, "CorrectOption1"},
                {question.second_option,"CorrectOption2" },
                {question.third_option,"CorrectOption3" },
                {question.fourth_option,"CorrectOption4"  }
            };

            List<KeyValuePair<string, string>> possibleOptionsList = ShuffleDicionaryOptions(possibleOptions);
            AssignOptionsToGUI(possibleOptionsList);
        }
        else
        {
            // The last case possible is that only exist one correct option.
            Dictionary<string, string> possibleOptions = new Dictionary<string, string>(){
                {question.first_option, "CorrectOption1"},
                {question.first_missing_word,"BadOption" },
                {question.second_missing_word,"BadOption" },
                {question.third_missing_word,"BadOption"  }
            };

            List<KeyValuePair<string, string>> possibleOptionsList = ShuffleDicionaryOptions(possibleOptions);
            AssignOptionsToGUI(possibleOptionsList);
        }
    }
}
