using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour

{

    [SerializeField] private Canvas canvas;


    // Start is called before the first frame update
    void Start()
    {
        // 1. Find the Minimap Camera in the scene

        GameObject miniMapcamera = GameObject.FindWithTag("Minimap");

        if(miniMapcamera != null)
        {
            // 2. Assign to prefab the camera found to Event Camera in the own canva
            canvas.worldCamera = miniMapcamera.GetComponent<Camera>();

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
