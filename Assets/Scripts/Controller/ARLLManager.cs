using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using Jace.Operations;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ARLLManager : MonoBehaviour
{

    [SerializeField] private GameObject canvasParentGO;
    [SerializeField] private GameObject banksParentGO;
    [SerializeField] private GameObject textToValidate;
    [SerializeField] private GameObject[] allChildrenCanvasGO;
    [SerializeField] private GameObject[] allChildrenBankParentGO;
    [SerializeField] private GameObject positiveFeedback;
    [SerializeField] private GameObject negativeFeedback;
    [SerializeField] private CalculatorController calculatorController;

    public UnityEvent feedbackPositiveEvent;
    public UnityEvent feedbackNegativeEvent;

    // Start is called before the first frame update
    void Start()
    {
        // ARLLWheelModel wheelTest =  ARLLWheelModel.GetARLLWheelById(2);
        // Debug.Log(wheelTest.created_at);
        // Debug.Log(wheelTest.arll_wheel_name);


        feedbackPositiveEvent.AddListener(() => {
            ActivatePositiveFeedbackGUI();
        }); 

        feedbackNegativeEvent.AddListener(() => {
            ActivateNegativeFeedbackGUI();
        }); 
    }

    public void TurnOnMinigame(string gameObjectToTurnOn, GameObject[] listToVerify)
    {
        foreach (GameObject child in listToVerify)
        {   
            if (child.name == gameObjectToTurnOn)
            {
                foreach (Transform grandchild in child.transform)
                {
                    grandchild.gameObject.SetActive(true);
                }
                break; 
            }
        }
    }

    private void TurnOffMinigame(string gameObjectToTurnOffChildren, GameObject[] listToVerify)
    {
        if (gameObjectToTurnOffChildren == "NoChildren")
        {
            return;
        }

        foreach (GameObject child in listToVerify)
        {
            if (child.name == gameObjectToTurnOffChildren)
            {
                foreach (Transform grandchild in child.transform)
                {
                    grandchild.gameObject.SetActive(false);
                }
                break;  
            }
        }
    }

    private bool HasActiveChildren(GameObject gameObject)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    private string GetNextCanvasName(int currentIndex, GameObject[] listToVerify)
    {
        if (currentIndex + 1 < listToVerify.Length)
        {
            GameObject nextChild = listToVerify[currentIndex + 1];
            if (nextChild != null)
            {
                return nextChild.name;
            }
            else
            {
                UnityEngine.Debug.LogError($"El siguiente GameObject en listToVerify[{currentIndex + 1}] es nulo.");
                return "Siguiente GameObject es nulo";
            }
        }
        UnityEngine.Debug.Log("No hay más GameObjects después del índice " + currentIndex);
        return "Fin de la lista alcanzado";
    }

    private string GetCurrentCanvasWithActiveChildren(GameObject[] listToVerify)
    {
        for (int i = 0; i < listToVerify.Length; i++)
        {
            GameObject currentChild = listToVerify[i];
            if (currentChild == null)
            {
                UnityEngine.Debug.LogError($"GameObject en listToVerify[{i}] es nulo.");
                continue;
            }

            if (HasActiveChildren(currentChild))
            {
                return currentChild.name; 
            }
        }
        return "NoChildren";
    }

    private string ValidateInWichOneCanvasActivate(GameObject[] listToVerify)
    {
        for (int i = 0; i < listToVerify.Length; i++)
        {
            GameObject currentChild = listToVerify[i];
            if (currentChild == null)
            {
                UnityEngine.Debug.LogError($"GameObject en listToVerify[{i}] es nulo.");
                continue;
            }

            if (HasActiveChildren(currentChild))
            {
                return GetNextCanvasName(i, listToVerify);
            }
        }
        return "No se encontraron GameObjects con hijos activos";
    }

    public void ValidateCalculatorResult()
    {
        string actualCanvasMinigame = GetCurrentCanvasWithActiveChildren(allChildrenBankParentGO);
        UnityEngine.Debug.Log(actualCanvasMinigame);
        switch (actualCanvasMinigame)
        {
            case "Weight_Bank":
                GameObject specificHeatGO = ObtainSpecificGameObject(actualCanvasMinigame, "Specific Heat");
                UnityEngine.Debug.Log(specificHeatGO.name);
                Text valueSpecificHeat = specificHeatGO.transform.GetChild(0).GetComponent<Text>();
                UnityEngine.Debug.Log(valueSpecificHeat.text);

                if (valueSpecificHeat.text != "---")
                {
                    TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();
                    string input = valueSpecificHeat.text; 
                    string numberString = Regex.Match(input, @"\d+").Value; 

                    int validationNumber;
                    if (int.TryParse(numberString, out validationNumber))
                    {
                        validationNumber *= 10; // If you need to multiply the number by 10
                        UnityEngine.Debug.Log("Parsed number: " + validationNumber);
                    }
                    string validationString = validationNumber.ToString();
                    if (textToValidateTheResult.text == validationString) { feedbackPositiveEvent.Invoke(); }
                    else { feedbackNegativeEvent.Invoke(); }
                }
                else { feedbackNegativeEvent.Invoke(); }
                
                break;
            case "InflateBank":
                break;
            case "Temperature_Bank":
                break;
            case "Work_Bank":
                break;
            case "E_DeltaE_Bank":
                break;
        }
    }

    private GameObject ObtainSpecificGameObject(string actualCanvas, string nameGameObject)
    {
        foreach (GameObject child in allChildrenBankParentGO)
        {   
            if (child.name == actualCanvas)
            {
                foreach (Transform grandchild in child.transform)
                {
                    if (grandchild.name == nameGameObject)
                    {
                        return grandchild.gameObject;
                    }
                }
                break; 
            }
        }
        return GameObject.Find(nameGameObject);
    }

    private TextMeshProUGUI ObtainActualTextForValidation()
    {
        return textToValidate.GetComponent<TextMeshProUGUI>();
    }

    public void ActivatePositiveFeedbackGUI()
    {
        positiveFeedback.SetActive(true);
    }

    public void OnPositiveFeedbackGUI()
    {
        TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();
        string canvasToActivate = ValidateInWichOneCanvasActivate(allChildrenCanvasGO);
        string banksToActivate = ValidateInWichOneCanvasActivate(allChildrenBankParentGO);
        UnityEngine.Debug.Log($"Bancos para activar: {banksToActivate} ");
        TurnOffMinigame(GetCurrentCanvasWithActiveChildren(allChildrenCanvasGO), allChildrenCanvasGO);
        TurnOffMinigame(GetCurrentCanvasWithActiveChildren(allChildrenBankParentGO), allChildrenBankParentGO);
        UnityEngine.Debug.Log(canvasToActivate);

        TurnOnMinigame(canvasToActivate, allChildrenCanvasGO);
        TurnOnMinigame(banksToActivate, allChildrenBankParentGO);
        textToValidateTheResult.text = "0";

        bool _isCalculatorHide = calculatorController.ObtainAnimationValue();
        if (_isCalculatorHide) { calculatorController.ToggleMovement(); }
        positiveFeedback.SetActive(false);
    }

    public void ActivateNegativeFeedbackGUI()
    {
        negativeFeedback.SetActive(true);
    }

    public void OnNegativeFeedbackGUI()
    {
        TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();
        textToValidateTheResult.text = "0";
        negativeFeedback.SetActive(false);
    }
}
