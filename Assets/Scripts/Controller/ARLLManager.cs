using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class ARLLManager : MonoBehaviour
{

    [SerializeField] private GameObject canvasParentGO;
    [SerializeField] private GameObject banksParentGO;
    private GameObject[] allChildrenCanvasGO;
    private List<string> allChildrenToDoNotTurnOn = new List<string> {
        "Button FdB Positive", "Button FdB Negative"
        };

    // Start is called before the first frame update
    void Start()
    {
        // ARLLWheelModel wheelTest =  ARLLWheelModel.GetARLLWheelById(2);
        // Debug.Log(wheelTest.created_at);
        // Debug.Log(wheelTest.arll_wheel_name);

        Transform[] allChildrenTransforms = canvasParentGO.transform.GetComponentsInChildren<Transform>(true);
        
        allChildrenCanvasGO = new GameObject[allChildrenTransforms.Length];
        for (int i = 0; i < allChildrenTransforms.Length; i++)
        {
            allChildrenCanvasGO[i] = allChildrenTransforms[i].gameObject;
            UnityEngine.Debug.Log(allChildrenTransforms[i].gameObject.name);
        }
        
        // TurnOnMinigame("----1. WheelWeight----");
        // TurnOnMinigame("Weight_Bank");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


public void TurnOnMinigame(string gameObjectToTurnOn)
{
    List<GameObject> objectsToKeepActive = new List<GameObject>(allChildrenCanvasGO);

    foreach (GameObject child in allChildrenCanvasGO)
    {   
        if (child.name == gameObjectToTurnOn)
        {
            Transform[] allChildrenToTurnOnTransforms = child.transform.GetComponentsInChildren<Transform>(true);
            foreach(Transform childToTurnOn in allChildrenToTurnOnTransforms)
            {
                if (!allChildrenToDoNotTurnOn.Contains(childToTurnOn.gameObject.name))
                {
                    childToTurnOn.gameObject.SetActive(true);
                    objectsToKeepActive.Add(childToTurnOn.gameObject);
                }
            }
            objectsToKeepActive.Add(child);
            TurnOffMinigame(objectsToKeepActive);
            break; 
        }
    }
}

private void TurnOffMinigame(List<GameObject> objectsToKeepActive)
{
    foreach (GameObject child in allChildrenCanvasGO)
    {
        if (!objectsToKeepActive.Contains(child))
        {
            child.gameObject.SetActive(false);
        }
    }
}
}
