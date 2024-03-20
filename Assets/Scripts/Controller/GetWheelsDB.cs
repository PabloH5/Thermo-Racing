using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWheelsDB : MonoBehaviour


{
    // Start is called before the first frame update
    void Start()
    {
        List<ARLLQuestionModel> wheels = ARLLQuestionModel.GetARLLQuestions();
        foreach (var wheel in wheels)
        {
            Debug.Log(wheel.number_moles);
        }
        Debug.Log(ARLLQuestionModel.GetARLLQuestionById(1).pressure);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
