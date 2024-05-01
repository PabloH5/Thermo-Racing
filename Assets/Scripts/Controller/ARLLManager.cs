using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using TMPro;
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

    public enum ErrorARLLmanager 
    {
        ErrorCalculator, 
        ErrorNotInteraction,
        ErrorInflateWheel,
        ErrorRotateWheel,
    }

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
    }

    /// <summary>
    /// Activates a specific minigame object within a list of game objects.
    /// </summary>
    /// <param name="gameObjectToTurnOn">The name of the game object to be activated.</param>
    /// <param name="listToVerify">Array of GameObjects that are checked for the specified game object.</param>
    /// <remarks>
    /// This method iterates through an array of GameObjects, checking each to find a match with 'gameObjectToTurnOn'.
    /// Once a match is found, it activates all child objects of the matched GameObject and stops further searching.
    /// </remarks>
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

    /// <summary>
    /// Deactivates the child game objects of a specific parent game object within a provided list.
    /// </summary>
    /// <param name="gameObjectToTurnOffChildren">The name of the parent game object whose children are to be deactivated.</param>
    /// <param name="listToVerify">Array of GameObjects that are checked for the specified parent game object.</param>
    /// <remarks>
    /// This method iterates through an array of GameObjects to find a specific game object by name.
    /// If the specified game object is found, all its child objects are deactivated.
    /// If 'gameObjectToTurnOffChildren' is set to "NoChildren", the method returns immediately without making any changes.
    /// This prevents the deactivation of children if the specific condition is met.
    /// </remarks>
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

    /// <summary>
    /// Determines whether any child objects of a given game object are currently active in the hierarchy.
    /// </summary>
    /// <param name="gameObject">The parent game object to check for active children.</param>
    /// <returns>True if at least one child object is active in the hierarchy; otherwise, false.</returns>
    /// <remarks>
    /// This method iterates through all child objects of the specified parent game object.
    /// It returns true as soon as it finds an active child object, indicating that not all children are inactive.
    /// If no active children are found after checking all, it returns false.
    /// </remarks>
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

    /// <summary>
    /// Retrieves the name of the next game object in an array, based on a given current index.
    /// </summary>
    /// <param name="currentIndex">The current index in the array from which to find the next game object.</param>
    /// <param name="listToVerify">The array of GameObjects to check.</param>
    /// <returns>The name of the next game object if available; otherwise, returns a message indicating no further objects or null.</returns>
    /// <remarks>
    /// This method checks if there is a next game object in the array beyond the current index.
    /// If the next game object exists and is not null, its name is returned.
    /// If the next game object is null, a specific message indicating that the next GameObject is null is returned.
    /// If the current index is at the end of the array, a message indicating the end of the list is reached is returned.
    /// </remarks>
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
                return "Siguiente GameObject es nulo";
            }
        }
        return "Fin de la lista alcanzado";
    }

    /// <summary>
    /// Searches an array of GameObjects and returns the name of the first GameObject with active children.
    /// </summary>
    /// <param name="listToVerify">The array of GameObjects to search through.</param>
    /// <returns>The name of the first GameObject with active children; returns "NoChildren" if none are found or if all GameObjects are null.</returns>
    /// <remarks>
    /// This method iterates through the provided array of GameObjects, checking each one for active children using the HasActiveChildren method.
    /// If a GameObject is null, it logs an error message and continues to the next GameObject in the array.
    /// It returns the name of the first GameObject that has active children. If no such GameObject is found, it returns "NoChildren".
    /// </remarks>
    private string GetCurrentCanvasWithActiveChildren(GameObject[] listToVerify)
    {
        for (int i = 0; i < listToVerify.Length; i++)
        {
            GameObject currentChild = listToVerify[i];
            if (currentChild == null)
            {
                continue;
            }

            if (HasActiveChildren(currentChild))
            {
                return currentChild.name; 
            }
        }
        return "NoChildren";
    }

    /// <summary>
    /// Determines which canvas should be activated next based on the first GameObject with active children in a given array.
    /// </summary>
    /// <param name="listToVerify">The array of GameObjects to inspect.</param>
    /// <returns>The name of the canvas that follows the first GameObject with active children; returns a message if no active children are found.</returns>
    /// <remarks>
    /// This method iterates through an array of GameObjects, searching for the first GameObject that has active children using the HasActiveChildren method.
    /// If a GameObject is found with active children, it retrieves the name of the next GameObject in the array using GetNextCanvasName.
    /// If a GameObject is null, it logs an error and continues to the next GameObject.
    /// If no GameObjects with active children are found, it returns a specific message indicating that no such GameObjects were found.
    /// </remarks>
    private string ValidateInWichOneCanvasActivate(GameObject[] listToVerify)
    {
        for (int i = 0; i < listToVerify.Length; i++)
        {
            GameObject currentChild = listToVerify[i];
            if (currentChild == null)
            {
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
                    else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorCalculator); }
                }
                else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorNotInteraction); }
                
                break;
            case "InflateBank":
                GameObject finalVolumeGO = ObtainSpecificGameObject(actualCanvasMinigame, "Final Volume");
                Text valueFinalVolume = finalVolumeGO.transform.GetChild(0).GetComponent<Text>();

                if (valueFinalVolume.text != "?")
                {
                    TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();
                    if (textToValidateTheResult.text == "0.49") { feedbackPositiveEvent.Invoke(); }
                    else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorCalculator); }   
                }
                else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorNotInteraction); }
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
                    else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorCalculator); }
                }
                else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorNotInteraction); }
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
                    else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorCalculator); }
                }
                else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorNotInteraction); }

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
                    else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorCalculator); }
                }
                else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorNotInteraction); }
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

    public void ActiveExplosion(ErrorARLLmanager errorARLL)
    {
        explosionGO.SetActive(true);
        explosionGO.GetComponent<ParticleSystem>().Play();
        explosionGO.GetComponent<AudioSource>().Play();
        StartCoroutine(ProcessExplosion(errorARLL));
    }

    public void ActivateGoodAudioBehaviour()
    {
        goodBehaviourAudioGO.SetActive(true);
        goodBehaviourAudioGO.GetComponent<AudioSource>().Play();
        StartCoroutine(DeactivateGameObject(1.0f, explosionGO));
    }

    private IEnumerator ProcessExplosion(ErrorARLLmanager errorARLL)
    {
        yield return StartCoroutine(DeactivateGameObject(1.5f, explosionGO));
        ActivateNegativeFeedbackGUI(errorARLL);
    }

    private IEnumerator DeactivateGameObject(float waitTime, GameObject goToDeactivate)
    {
        yield return new WaitForSeconds(waitTime);
        goToDeactivate.SetActive(false);
    }

    public void ActivateNegativeFeedbackGUI(ErrorARLLmanager error)
    {
        negativeFeedback.SetActive(true);
        switch (error)
        {
            case ErrorARLLmanager.ErrorCalculator:
                ModifyNegativeFeedback("Tu cálculo no es correcto, por favor vuelve a intentarlo.");
            break;
            case ErrorARLLmanager.ErrorNotInteraction:
                ModifyNegativeFeedback("No has desbloqueado tu variable objetivo, por favor interactúa antes de utilizar la calculadora.");
            break;
            case ErrorARLLmanager.ErrorInflateWheel:
                ModifyNegativeFeedback("Debes de moderar la cantidad de aire que tiene la llanta. Vuelve a intentarlo.");
            break;
            case ErrorARLLmanager.ErrorRotateWheel:
                ModifyNegativeFeedback("La llanta alcanzó una temperatura muy alta, ten cuidado la próxima vez.");
            break;
            default:
                ModifyNegativeFeedback("Por favor vuelve a intentarlo.");
            break;
        }
    }

    private void ModifyNegativeFeedback(string textToModify)
    {
        Transform childTxtFeedback = negativeFeedback.transform.GetChild(0);
        childTxtFeedback.GetComponent<Text>().text = textToModify;
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
