using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARLLManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ARLLWheelModel wheelTest =  ARLLWheelModel.GetARLLWheelById(2);
        Debug.Log(wheelTest.created_at);
        Debug.Log(wheelTest.arll_wheel_name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
