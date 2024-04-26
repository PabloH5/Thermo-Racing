using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conexion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextAsset jsonTextFile = Resources.Load<TextAsset>("config.local");
        string jsonContent = jsonTextFile.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
