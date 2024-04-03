using Jace;
using System;
using TMPro;
using UnityEngine;

public class CalculatorController : MonoBehaviour
{
    private TextMeshProUGUI _OperationInputField;
    public TextMeshProUGUI operationInputField
    {
        get { return _OperationInputField; }
        set { _OperationInputField = value; }
    }

    //private float numbers = 0;

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
                Debug.Log("I did it");
                Debug.Log(operationInputField.GetType().ToString());
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
        if (operationInputField.text == "0" || ValidateDobleSymbolOperation(operationInputField.text))
        {
            return;
        }

        operationInputField.text += operationSymbol;
    }

    public void BtnDigit(string numberToPut)
    {
        Debug.Log(numberToPut.GetType().ToString());
        if (operationInputField.text == Convert.ToString("0"))
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
        if (operationInputField.text.Length > 1) { operationInputField.text = operationInputField.text.Substring(0, operationInputField.text.Length - 1); }
        else { operationInputField.text = "0"; }
    }

    public void EvaluateTheOperation()
    {
        var engine = new CalculationEngine();

        try
        {
            double result = Math.Round(engine.Calculate(operationInputField.text), 2);
            operationInputField.text = result.ToString();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error during evaluation the expression: {e.Message}");
            operationInputField.text = "0";
        }
    }
}
