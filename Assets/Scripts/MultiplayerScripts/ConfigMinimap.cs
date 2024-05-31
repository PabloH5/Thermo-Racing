using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigMinimap : MonoBehaviour
{
    private Canvas canvasMinimap;
    private Camera cameraMinimap;


    // Start is called before the first frame update
    void Start()
    {
        cameraMinimap = GameObject.FindGameObjectWithTag("CameraMinimap").GetComponent<Camera>();
        canvasMinimap = GetComponent<Canvas>();
        canvasMinimap.renderMode = RenderMode.ScreenSpaceOverlay;
        // canvasMinimap.worldCamera = cameraMinimap;
    }
}
