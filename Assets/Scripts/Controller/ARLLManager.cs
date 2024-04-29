using UnityEngine;
using UnityEngine.Events;
using System;
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
    [SerializeField] private GameObject finalFeedback;
    [SerializeField] private CalculatorController calculatorController;
    [SerializeField] private DragRotate dragRotateController;
    [SerializeField] private ConstantBankUpdate constantBankUpdateController;
    [SerializeField] private GameObject calculatorToggleGO;
    [SerializeField] private GameObject explosionGO;
    [SerializeField] private GameObject goodBehaviourAudioGO;

    [Space(5)]
    [Header("BD informarion")]
    private ARLLQuestionModel questionBD;


    private WheelType _wheelTypeController;
    public WheelType wheelTypeController
    {
        get { return _wheelTypeController; }
        set { _wheelTypeController = value; }
    }

    private float _WheelSpecificHeat;
    public float wheelSpecificHeat
    {
        get { return _WheelSpecificHeat; }
        set { _WheelSpecificHeat = value; }
    }

    public UnityEvent feedbackPositiveEvent;
    public UnityEvent feedbackNegativeEvent;

    // Start is called before the first frame update
    void Start()
    {
        //ARLLWheelModel wheelTest = ARLLWheelModel.GetARLLWheelById(2);
        // Debug.Log(wheelTest.created_at);
        // Debug.Log(wheelTest.arll_wheel_name);

        // Bring the information from BD
        // questionBD = ARLLQuestionModel.GetARLLQuestionById(1);
        // UnityEngine.Debug.Log($"Hola BD: {questionBD.change_internal_energy} - {questionBD.arll_wheel_name}");


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
                GameObject specificHeatGO = ObtainSpecificGameObject(actualCanvasMinigame, "Specific Heat Pression");
                Text valueSpecificHeat = specificHeatGO.transform.GetChild(0).GetComponent<Text>();

                if (valueSpecificHeat.text != "?")
                {
                    TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();
                    string input = valueSpecificHeat.text; 
                    string numberString = Regex.Match(input, @"\d+").Value; 

                    int validationNumber;
                    if (int.TryParse(numberString, out validationNumber))
                    {
                        validationNumber *= 10;
                    }
                    string validationString = validationNumber.ToString();
                    if (textToValidateTheResult.text == validationString) { feedbackPositiveEvent.Invoke(); }
                    else { feedbackNegativeEvent.Invoke(); }
                }
                else { feedbackNegativeEvent.Invoke(); }
                
                break;
            case "InflateBank":
                GameObject finalVolumeGO = ObtainSpecificGameObject(actualCanvasMinigame, "Final Volume");
                Text valueFinalVolume = finalVolumeGO.transform.GetChild(0).GetComponent<Text>();

                if (valueFinalVolume.text != "?")
                {
                    TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();
                    if (textToValidateTheResult.text == "0.49") { feedbackPositiveEvent.Invoke(); }
                    else { feedbackNegativeEvent.Invoke(); }   
                }
                break;
            case "Temperature_Bank":
                GameObject finalTemperatureGO = ObtainSpecificGameObject(actualCanvasMinigame, "Final Temp");
                Text valueFinalTemperature = finalTemperatureGO.transform.GetChild(0).GetComponent<Text>();

                if (valueFinalTemperature.text != "?")
                {
                    TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();
                    string input = valueFinalTemperature.text; 
                    string numberString = Regex.Match(input, @"\d+").Value; 

                    float validationNumber;
                    if (float.TryParse(numberString, out validationNumber))
                    {
                        validationNumber = wheelSpecificHeat * 10.0f * 55.0f; 
                    }
                    string validationString = validationNumber.ToString();
                    if (textToValidateTheResult.text == validationString) { feedbackPositiveEvent.Invoke(); }
                    else { feedbackNegativeEvent.Invoke(); }
                }
                else { feedbackNegativeEvent.Invoke(); }
                break;
            case "Work_Bank":
                break;
            case "E_Bank":
                GameObject finalHeatAmountGO = ObtainSpecificGameObject(actualCanvasMinigame, "Heat amount");
                Text valueFinalHeatAmount = finalHeatAmountGO.transform.GetChild(0).GetComponent<Text>();

                if (valueFinalHeatAmount.text != "?")
                {
                    TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();
                    string input = valueFinalHeatAmount.text; 
                    string numberString = Regex.Match(input, @"\d+").Value; 

                    float validationNumber = (float)Math.Round(992.985f / (wheelSpecificHeat * 10f * 55f) * 100, 2);
                    UnityEngine.Debug.Log(validationNumber);
                    
                    string validationString = validationNumber.ToString();
                    if (textToValidateTheResult.text == validationString) { feedbackPositiveEvent.Invoke(); }
                    else { feedbackNegativeEvent.Invoke(); }
                }
                else { feedbackNegativeEvent.Invoke(); }

                break;
            case "DeltaE_Bank":
                GameObject finalEfficiencyGO = ObtainSpecificGameObject(actualCanvasMinigame, "e");
                Text valueEfficiency = finalEfficiencyGO.transform.GetChild(0).GetComponent<Text>();

                if (valueEfficiency.text != "?")
                {
                    TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();

                    float validationNumber = (float)Math.Round((wheelSpecificHeat * 10f * 55f) + 992.985f, 2);
                    UnityEngine.Debug.Log(validationNumber);
                    
                    string validationString = validationNumber.ToString();
                    if (textToValidateTheResult.text == validationString) { finalFeedback.SetActive(true); }
                    else { feedbackNegativeEvent.Invoke(); }
                }
                else { feedbackNegativeEvent.Invoke(); }
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
        TurnOffMinigame(GetCurrentCanvasWithActiveChildren(allChildrenCanvasGO), allChildrenCanvasGO);
        TurnOffMinigame(GetCurrentCanvasWithActiveChildren(allChildrenBankParentGO), allChildrenBankParentGO);

        TurnOnMinigame(canvasToActivate, allChildrenCanvasGO);
        TurnOnMinigame(banksToActivate, allChildrenBankParentGO);
        textToValidateTheResult.text = "0";

        string nextCanvas = ValidateInWichOneCanvasActivate(allChildrenCanvasGO);    
        if (nextCanvas == "----3. WheelRotation----")
        {
            dragRotateController.ModifyEnumAtribute(wheelTypeController);
        }

        string actualCanvas = GetCurrentCanvasWithActiveChildren(allChildrenCanvasGO);
        string actualBank = GetCurrentCanvasWithActiveChildren(allChildrenBankParentGO); 
        bool _isCalculatorHide = calculatorController.ObtainAnimationValue();
    
        if (actualCanvas == "----5. LastCalculations----")
        {
            GameObject finalSpecificHeatGO = ObtainSpecificGameObject(actualBank, "Heat amount");
            UnityEngine.Debug.Log(actualBank);
            Text valueFinalSpecificHeat = finalSpecificHeatGO.transform.GetChild(0).GetComponent<Text>();
            valueFinalSpecificHeat.text = $"{wheelSpecificHeat * 10 * 55}" + " J/(kg·°K)";

            if (!_isCalculatorHide) 
            { 
                calculatorController.ToggleMovement(); 
            }
            calculatorToggleGO.SetActive(false);
        }

        if (actualCanvas == "----6. LastCalculations----")
        {

            GameObject finalEfficiencyGO = ObtainSpecificGameObject(actualBank, "e");
            Text valueFinalEfficiency = finalEfficiencyGO.transform.GetChild(0).GetComponent<Text>();
            valueFinalEfficiency.text = $"{(float)Math.Round(992.985f / (wheelSpecificHeat * 10f * 55f) * 100, 2)}" + "%";

            float finalHeatAmountDeltaE = wheelSpecificHeat * 10.0f * 55.0f;
            GameObject finalHeatAmountDeltaEGO = ObtainSpecificGameObject(actualBank, "Heat amount");
            Text valueFinalHeatAmountDeltaEText = finalHeatAmountDeltaEGO.transform.GetChild(0).GetComponent<Text>();
            valueFinalHeatAmountDeltaEText.text = finalHeatAmountDeltaE.ToString() + " J/(kg·°K)";

            if (_isCalculatorHide) 
            { 
                calculatorController.ToggleMovement(); 
            }
        }
        
        if (_isCalculatorHide) { calculatorController.ToggleMovement();  }

        positiveFeedback.SetActive(false);
    }

    public void ActiveExplosion()
    {
        explosionGO.SetActive(true);
        explosionGO.GetComponent<ParticleSystem>().Play();
        StartCoroutine(ProcessExplosion());
    }

    public void ActivateGoodAudioBehaviour()
    {
        goodBehaviourAudioGO.SetActive(true);
        goodBehaviourAudioGO.GetComponent<AudioSource>().Play();
        StartCoroutine(DeactivateGameObject(1.0f, explosionGO));
    }

    private IEnumerator ProcessExplosion()
    {
        yield return StartCoroutine(DeactivateGameObject(1.0f, explosionGO));
        ActivateNegativeFeedbackGUI();
    }

    private IEnumerator DeactivateGameObject(float waitTime, GameObject goToDeactivate)
    {
        yield return new WaitForSeconds(waitTime);
        goToDeactivate.SetActive(false);
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

    public WheelType ParseWheelType(string tireName)
    {
        switch (tireName)
        {
            case "Caucho Natural":
                wheelSpecificHeat = 2000f;
                constantBankUpdateController.UpdateSpecificHeat(wheelSpecificHeat);
                return WheelType.Natural;
            case "Caucho Sintético":
                wheelSpecificHeat = 1600f;
                constantBankUpdateController.UpdateSpecificHeat(wheelSpecificHeat);
                return WheelType.Synthectic;
            case "Caucho Semisintético":
                wheelSpecificHeat = 1800f;
                constantBankUpdateController.UpdateSpecificHeat(wheelSpecificHeat);
                return WheelType.Semisynthectic;
            default:
                UnityEngine.Debug.LogError("Unknown tire type: " + tireName);
                return wheelTypeController; // Return current value if not matched
        }
    }

    public void SetWheelTypeInController(WheelType wheelType)
    {
        wheelTypeController = wheelType;
    }
}
