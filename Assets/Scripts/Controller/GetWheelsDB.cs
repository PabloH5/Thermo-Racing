using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWheelsDB : MonoBehaviour


{
    // Start is called before the first frame update
    void Start()
    {
        List<WheelModel> wheels = WheelModel.GetWheels();
        //foreach (var wheel in wheels)
        //{
        //    Debug.Log(wheel.wheel_name);
        //}
        Debug.Log(WheelModel.GetWheelById(1).wheel_name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
