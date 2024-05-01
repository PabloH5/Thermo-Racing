using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<RaceQuestionModel> raceQuestions = RaceQuestionModel.GetRaceQuestions();
        raceQuestions.ForEach(question => Debug.Log(question.race_question_id));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
