using Jace;
using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class CalculatorController : MonoBehaviour
{
    private GameObject calculatorBackground;
    private bool _isToggleCalculatorNotVisible = false;
    private TextMeshProUGUI _OperationInputField;
    public TextMeshProUGUI operationInputField
    {
        get { return _OperationInputField; }
        set { _OperationInputField = value; }
    }

    private Animator toggleAnimation;
    [SerializeField] string operation;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] allChildren = this.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if (child.name == "CalculationText")
            {
                operationInputField = child.gameObject.GetComponent<TextMeshProUGUI>();
                Debug.Log("I did it for the CalculationText");
            }
            if (child.name == "CalculatorBackground")
            {
                calculatorBackground = child.gameObject;
                toggleAnimation = calculatorBackground.GetComponent<Animator>();
                toggleAnimation.speed = 0f;
                Debug.Log("I did it for the Calculator");
            }
        }
    }

    public void Addition()
    {
        AppendOperationIfValid("+");
    }

    public void Sustraction()
    {
        AppendOperationIfValid("-");
    }

    public void Multiplication()
    {
        AppendOperationIfValid("*");
    }

    public void Division()
    {
        AppendOperationIfValid("/");
    }

    public void Dot()
    {
        AppendOperationIfValid(".");
    }

    private void AppendOperationIfValid(string operationSymbol)
    {
        if ((operationInputField.text == "0" || ValidateDobleSymbolOperation(operationInputField.text))
            && operationInputField.text.Length >= 42)
        {
            return;
        }

        operationInputField.text += operationSymbol;
    }

    public void BtnDigit(string numberToPut)
    {
        if (operationInputField.text.Length >= 44)
        {
            return;
        }

        if (operationInputField.text == "0")
        {
            operationInputField.text = numberToPut;
        }
        else if (ValidateDobleParenthesis(operationInputField.text))
        {
            if (numberToPut == "(" || numberToPut == ")") { }
            else { operationInputField.text = operationInputField.text + numberToPut; }
        }
        else
        {
            operationInputField.text = operationInputField.text + numberToPut;
        }
    }

    private bool ValidateDobleSymbolOperation(string allText)
    {
        if (allText.EndsWith("+")) { return true; }
        else if (allText.EndsWith("-")) { return true; }
        else if (allText.EndsWith("*")) { return true; }
        else if (allText.EndsWith("/")) { return true; }
        else if (allText.EndsWith(".")) { return true; }
        else { return false; }
    }

    private bool ValidateDobleParenthesis(string allText)
    {
        if (allText.EndsWith("(")) { return true; }
        else if (allText.EndsWith(")")) { return true; }
        else if (allText.EndsWith(".")) { return true; }
        else { return false; }
    }

    public void BtnClear()
    {
        operationInputField.text = "0";
    }

    public void BtnClearLastDigit()
    {
        if (operationInputField.text.Length > 1)
        {
            operationInputField.text = operationInputField.text.Substring(0, operationInputField.text.Length - 1);
        }
        else
        {
            operationInputField.text = "0";
        }
    }

    private string NormalizeDecimalSeparators(string input)
    {
        return input.Replace('.', ',');
    }

    public void EvaluateTheOperation()
    {
        var engine = new CalculationEngine();

        // Obtain the current culture of the system
        CultureInfo currentCulture = CultureInfo.CurrentCulture;

        // Normalise the expression if the language is not en-US
        string expressionToEvaluate = operationInputField.text;
        if (currentCulture.Name != "en-US")
        {
            expressionToEvaluate = NormalizeDecimalSeparators(operationInputField.text);
        }

        try
        {
            
            double result = Math.Round(engine.Calculate(expressionToEvaluate), 2);

            operationInputField.text = result.ToString(CultureInfo.CurrentCulture);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error during evaluation the expression: {e.Message}");
            operationInputField.text = "0";
        }
    }

    public void ToggleMovement()
    {
        if (toggleAnimation != null)
        {
            toggleAnimation.speed = 1;
            if (_isToggleCalculatorNotVisible)
            {
                toggleAnimation.Play("CalculatorToggleAnimationHide", 0, 0);
            }
            else
            {
                toggleAnimation.Play("CalculatorToggleAnimationShow", 0, 0);
            }
            _isToggleCalculatorNotVisible = !_isToggleCalculatorNotVisible;
        }
    }

    public bool ObtainAnimationValue()
    {
        return _isToggleCalculatorNotVisible;
    }
}
