using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class ARLLManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject canvasParentGO;
    [SerializeField] private GameObject banksParentGO;
    [SerializeField] private GameObject textToValidate;
    [SerializeField] private GameObject[] allChildrenCanvasGO;
    [SerializeField] private GameObject[] allChildrenBankParentGO;
    [SerializeField] private GameObject positiveFeedback;
    [SerializeField] private GameObject negativeFeedback;
    
    [SerializeField] private CalculatorController calculatorController;
    [SerializeField] private DragRotate dragRotateController;
    [SerializeField] private ConstantBankUpdate constantBankUpdateController;
    [SerializeField] private GameObject calculatorToggleGO;
    [SerializeField] private GameObject explosionGO;
    [SerializeField] private GameObject goodBehaviourAudioGO;

    [Space(10)]
    [Header("Awards")]
    [SerializeField] private AwardsController awardController;
    [SerializeField] private GameObject finalFeedback;

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

    public enum ErrorARLLmanager 
    {
        ErrorCalculator, 
        ErrorNotInteraction,
        ErrorInflateWheel,
        ErrorRotateWheel,
    }
    #endregion

    #region Utilitie Methods

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

    /// <summary>
    /// Validates the temperature settings in the "Weight Bank" canvas.
    /// </summary>
    /// <param name="canvasName">The name of the canvas being validated.</param>
    /// <remarks>
    /// Retrieves the specific GameObject for the "Specific Heat Pression", extracts its value, and validates it
    /// by multiplying it with a predefined multiplier. Positive or negative feedback is triggered based on the result.
    /// </remarks>
    private void ValidateWeightBank(string canvasName)
    {
        GameObject specificHeatGO = ObtainSpecificGameObject(canvasName, "Specific Heat Pression");
        Text valueSpecificHeat = specificHeatGO.transform.GetChild(0).GetComponent<Text>();
        ValidateNumberValue(valueSpecificHeat, multiplier: wheelSpecificHeat * 10.0f);
    }

    /// <summary>
    /// Validates the temperature settings in the "Weight Bank" canvas.
    /// </summary>
    /// <param name="canvasName">The name of the canvas being validated.</param>
    /// <remarks>
    /// Retrieves the specific GameObject for the "Specific Heat Pression", extracts its value, and validates it
    /// by multiplying it with a predefined multiplier. Positive or negative feedback is triggered based on the result.
    /// </remarks>
    private void ValidateInflateBank(string canvasName)
    {
        GameObject finalVolumeGO = ObtainSpecificGameObject(canvasName, "Final Volume");
        Text valueFinalVolume = finalVolumeGO.transform.GetChild(0).GetComponent<Text>();
        ValidateNumberValue(valueFinalVolume, multiplier: 0.49f);
    }

    /// <summary>
    /// Validates the temperature settings in the "Temperature Bank" canvas.
    /// </summary>
    /// <param name="canvasName">The name of the canvas being validated.</param>
    /// <remarks>
    /// Retrieves the specific GameObject for "Final Temp", extracts its temperature value, and validates it
    /// by applying a specific multiplier to the extracted number. Feedback is triggered based on the validation.
    /// </remarks>
    private void ValidateTemperatureBank(string canvasName)
    {
        GameObject finalTemperatureGO = ObtainSpecificGameObject(canvasName, "Final Temp");
        Text valueFinalTemperature = finalTemperatureGO.transform.GetChild(0).GetComponent<Text>();
        ValidateNumberValue(valueFinalTemperature, multiplier: wheelSpecificHeat * 10.0f * 55.0f);
    }

    /// <summary>
    /// Validates the heat calculations in the "E Bank" canvas.
    /// </summary>
    /// <param name="canvasName">The name of the canvas being validated.</param>
    /// <remarks>
    /// Retrieves the specific GameObject for "Heat amount", extracts its value, and validates it
    /// by applying a rounded multiplier to the extracted number. Feedback is provided based on the validation outcome.
    /// </remarks>
    private void ValidateEBank(string canvasName)
    {
        GameObject finalHeatAmountGO = ObtainSpecificGameObject(canvasName, "Heat amount");
        Text valueFinalHeatAmount = finalHeatAmountGO.transform.GetChild(0).GetComponent<Text>();
        ValidateNumberValue(valueFinalHeatAmount, multiplier: (float)Math.Round(992.985f / (wheelSpecificHeat * 10f * 55f) * 100, 2));
    }

    /// <summary>
    /// Validates the efficiency calculations in the "DeltaE Bank" canvas.
    /// </summary>
    /// <param name="canvasName">The name of the canvas being validated.</param>
    /// <remarks>
    /// Retrieves the specific GameObject for "e", extracts its efficiency value, and validates it
    /// by applying a rounded multiplier. If the validation passes, final feedback is triggered; otherwise, negative feedback is activated.
    /// </remarks>
    private void ValidateDeltaEBank(string canvasName)
    {
        GameObject finalEfficiencyGO = ObtainSpecificGameObject(canvasName, "e");
        Text valueEfficiency = finalEfficiencyGO.transform.GetChild(0).GetComponent<Text>();
        ValidateNumberValue(valueEfficiency, multiplier: (float)Math.Round((wheelSpecificHeat * 10f * 55f) + 992.985f, 2), finalFeedbackBool: true);
    }

    /// <summary>
    /// Retrieves a specific GameObject based on its name within a specified canvas.
    /// </summary>
    /// <param name="actualCanvas">The name of the canvas (parent GameObject) where the search should begin.</param>
    /// <param name="nameGameObject">The name of the GameObject to retrieve.</param>
    /// <returns>The GameObject if found within the specified canvas; otherwise, attempts to find it globally in the scene.</returns>
    /// <remarks>
    /// This method iterates through all children of a defined parent GameObject array to locate a specific canvas.
    /// Once the canvas is found, it further iterates through its children to find the desired GameObject by name.
    /// If the GameObject is not found within the canvas, it falls back to a global search using GameObject.Find.
    /// This approach ensures that even if the GameObject is not located in the expected parent, it can still be retrieved,
    /// albeit with a potential performance cost due to the use of GameObject.Find.
    /// </remarks>
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

    /// <summary>
    /// Manages the activation and deactivation of canvases and bank elements based on their current validation states.
    /// </summary>
    /// <remarks>
    /// This method evaluates which canvas and bank should be active by validating their current states.
    /// It then deactivates the currently active canvas and bank and activates the next appropriate ones.
    /// This is crucial for ensuring the GUI reflects the current state of user interactions and game logic.
    /// </remarks>
    private void ManageCanvasAndBankActivity()
    {
        string canvasToActivate = ValidateInWichOneCanvasActivate(allChildrenCanvasGO);
        string banksToActivate = ValidateInWichOneCanvasActivate(allChildrenBankParentGO);
        TurnOffMinigame(GetCurrentCanvasWithActiveChildren(allChildrenCanvasGO), allChildrenCanvasGO);
        TurnOffMinigame(GetCurrentCanvasWithActiveChildren(allChildrenBankParentGO), allChildrenBankParentGO);
        TurnOnMinigame(canvasToActivate, allChildrenCanvasGO);
        TurnOnMinigame(banksToActivate, allChildrenBankParentGO);
    }

    private TextMeshProUGUI ObtainActualTextForValidation()
    {
        return textToValidate.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Validates a numeric value against a calculated multiplier and provides feedback based on the result.
    /// </summary>
    /// <param name="valueText">The text component containing the value to validate.</param>
    /// <param name="multiplier">The multiplier used to calculate the expected value.</param>
    /// <param name="finalFeedbackBool">Indicates whether to activate the final feedback if validation is successful.</param>
    /// <remarks>
    /// This method checks if the text value is not a placeholder. If valid, it compares the text against a calculated string.
    /// Positive or negative feedback is provided based on whether the values match.
    /// </remarks>
    private void ValidateNumberValue(Text valueText, float multiplier, bool finalFeedbackBool = false)
    {
        if (valueText.text != "?")
        {
            TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();
            string validationString = multiplier.ToString();
            if (textToValidateTheResult.text == validationString)
            {
                if (finalFeedbackBool) {
                    // WIN THE GAME
                    awardController.GetRandomAward();
                    finalFeedback.SetActive(true);
                }
                else { ActivatePositiveFeedbackGUI(); }
            }
            else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorCalculator); }
        }
        else { ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorNotInteraction); }
    }

    /// <summary>
    /// Modifies the text of the negative feedback message.
    /// </summary>
    /// <param name="textToModify">The new message to display as negative feedback.</param>
    /// <remarks>
    /// This method finds the text component within the negative feedback UI element and updates its content with the specified message.
    /// </remarks>
    private void ModifyNegativeFeedback(string textToModify)
    {
        Transform childTxtFeedback = negativeFeedback.transform.GetChild(0);
        childTxtFeedback.GetComponent<Text>().text = textToModify;
    }

        /// <summary>
    /// Activates the explosion effects and starts the explosion process.
    /// </summary>
    /// <param name="errorARLL">The type of error associated with the explosion, used to determine the feedback after the explosion.</param>
    /// <remarks>
    /// This method activates the explosion game object here and in other scripts, specifically in DragRotate and TouchAirBomb, 
    /// plays its particle system and audio source, and then starts a coroutine to process the 
    /// explosion effects and handle subsequent feedback based on the error type.
    /// </remarks>
    public void ActiveExplosion(ErrorARLLmanager errorARLL)
    {
        explosionGO.SetActive(true);
        explosionGO.GetComponent<ParticleSystem>().Play();
        explosionGO.GetComponent<AudioSource>().Play();
        StartCoroutine(ProcessExplosion(errorARLL));
    }

    /// <summary>
    /// Activates and plays the audio for good behavior feedback.
    /// </summary>
    /// <remarks>
    /// This method activates the good behavior audio game object, plays its audio source, and schedules its deactivation 
    /// after a set duration using a coroutine.
    /// </remarks>
    public void ActivateGoodAudioBehaviour()
    {
        goodBehaviourAudioGO.SetActive(true);
        goodBehaviourAudioGO.GetComponent<AudioSource>().Play();
        StartCoroutine(DeactivateGameObject(1.0f, explosionGO));
    }

    /// <summary>
    /// Deactivates a specified game object after a delay.
    /// </summary>
    /// <param name="waitTime">The time in seconds to wait before deactivating the game object.</param>
    /// <param name="goToDeactivate">The game object to deactivate.</param>
    /// <returns>An IEnumerator suitable for coroutine sequencing in Unity.</returns>
    /// <remarks>
    /// This coroutine waits for the specified duration before setting the game object's active state to false.
    /// </remarks>
    private IEnumerator DeactivateGameObject(float waitTime, GameObject goToDeactivate)
    {
        yield return new WaitForSeconds(waitTime);
        goToDeactivate.SetActive(false);
    }

    #endregion

    #region CalculatorLogic

    /// <summary>
    /// Validates the calculation results based on the active children in the minigame canvas.
    /// </summary>
    /// <remarks>
    /// Determines which canvas is currently active, logs the canvas name, and delegates the validation 
    /// to specific methods based on the active canvas. If the canvas corresponds to a specific game 
    /// scenario, specific validation logic is executed. Provides feedback based on the validation results.
    /// </remarks>
    public void ValidateCalculatorResult()
    {
        string actualCanvasMinigame = GetCurrentCanvasWithActiveChildren(allChildrenBankParentGO);
        switch (actualCanvasMinigame)
        {
            case "Weight_Bank":
                ValidateWeightBank(actualCanvasMinigame);
                break;
            case "InflateBank":
                ValidateInflateBank(actualCanvasMinigame);
                break;
            case "Temperature_Bank":
                ValidateTemperatureBank(actualCanvasMinigame);
                break;
            case "Work_Bank":
                // Implement if needed
                break;
            case "E_Bank":
                ValidateEBank(actualCanvasMinigame);
                break;
            case "DeltaE_Bank":
                ValidateDeltaEBank(actualCanvasMinigame);
                break;
            default:
                ActivateNegativeFeedbackGUI(ErrorARLLmanager.ErrorNotInteraction);
                break;
        }
    }

    #endregion

    #region Feedback

    public void ActivatePositiveFeedbackGUI()
    {
        positiveFeedback.SetActive(true);
    }
    
    /// <summary>
    /// Handles the positive feedback for the GUI and manages transitions between different canvases and banks.
    /// </summary>
    /// <remarks>
    /// This method is used for the PositiveFeedback Game object that has a button. It resets the validation text, 
    /// manages the activity state of canvases and banks, checks for specific canvas actions related to wheel rotation, 
    /// and updates calculation results based on the active canvas. It also toggles the calculator's visibility and deactivates the
    /// positive feedback display at the end of the process.
    /// </remarks>
    public void OnPositiveFeedbackGUI()
    {
        TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();
        textToValidateTheResult.text = "0";
        ManageCanvasAndBankActivity();

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

    /// <summary>
    /// Activates the GUI for negative feedback and updates its text based on the specified error.
    /// </summary>
    /// <param name="error">The error type that determines the feedback message.</param>
    /// <remarks>
    /// This method activates the negative feedback UI element and sets the feedback text based on the provided error type, customizing the response to various conditions.
    /// </remarks>
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

    /// <summary>
    /// Resets the validation text and hides the negative feedback UI.
    /// </summary>
    /// <remarks>
    /// This method is called to reset the feedback mechanism by setting the validation text to "0" and deactivating the negative feedback game object.
    /// </remarks>
    public void OnNegativeFeedbackGUI()
    {
        TextMeshProUGUI textToValidateTheResult = ObtainActualTextForValidation();
        textToValidateTheResult.text = "0";
        negativeFeedback.SetActive(false);
    }

    /// <summary>
    /// Activates and plays the audio for good behavior feedback.
    /// </summary>
    /// <remarks>
    /// This method activates the good behavior audio game object, plays its audio source, and schedules its deactivation 
    /// after a set duration using a coroutine.
    /// </remarks>
    private IEnumerator ProcessExplosion(ErrorARLLmanager errorARLL)
    {
        yield return StartCoroutine(DeactivateGameObject(1.5f, explosionGO));
        ActivateNegativeFeedbackGUI(errorARLL);
    }

    #endregion

    #region WheelType

    /// <summary>
    /// Parses the tire name and updates the wheel type based on the specified tire material.
    /// </summary>
    /// <param name="tireName">The name of the tire type which determines the specific heat and wheel type.</param>
    /// <returns>Returns the appropriate WheelType enum based on the tire material.</returns>
    /// <remarks>
    /// This method updates the wheel's specific heat value based on the tire type and logs an error if the tire type is unknown.
    /// It uses a switch statement to assign different specific heat values and corresponding 
    /// WheelTypes for natural, synthetic, and semisynthetic rubber.
    /// </remarks>
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

    /// <summary>
    /// Sets the wheel type in a controller.
    /// </summary>
    /// <param name="wheelType">The wheel type to set in the controller.</param>
    /// <remarks>
    /// This method updates the wheelTypeController with the provided wheel type. It is typically called after parsing the wheel type
    /// to synchronize the wheel type state across the application.
    /// </remarks>
    public void SetWheelTypeInController(WheelType wheelType)
    {
        wheelTypeController = wheelType;
    }

    #endregion
}