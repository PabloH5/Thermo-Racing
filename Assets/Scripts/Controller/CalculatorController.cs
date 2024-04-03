using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Jace;

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
        if (operationInputField.text == Convert.ToString("0"))
        {}
        else if (ValidateDobleSymbolOperation(operationInputField.text))
        { }
        else
        {
            operationInputField.text = operationInputField.text + ("+");
        }
    }

    public void Sustraction()
    {
        if (operationInputField.text == Convert.ToString("0"))
        { }
        else if (ValidateDobleSymbolOperation(operationInputField.text))
        { }
        else
        {
            operationInputField.text = operationInputField.text + ("-");
        }
    }

    public void Multiplication()
    {
        if (operationInputField.text == Convert.ToString("0"))
        { }
        else if(ValidateDobleSymbolOperation(operationInputField.text))
        { }
        else
        {
            operationInputField.text = operationInputField.text + ("*");
        }
    }

    public void Division()
    {
        if (operationInputField.text == Convert.ToString("0"))
        { }
        else if (ValidateDobleSymbolOperation(operationInputField.text))
        { }
        else
        {
            operationInputField.text = operationInputField.text + ("/");
        }
    }

    public void BtnDigit(string numberToPut)
    {
        Debug.Log(numberToPut.GetType().ToString());
        if (operationInputField.text == Convert.ToString("0"))
        {
            operationInputField.text = numberToPut;
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

    public void BtnClear()
    {
        operationInputField.text = "0";
    }

    public void BtnClearLastDigit()
    {
        operationInputField.text = "1";
    }

    public void EvaluateTheOperation() 
    {
        var engine = new CalculationEngine();

        try
        {
            double result = engine.Calculate(operationInputField.text);
            operationInputField.text = result.ToString();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error during evaluation the expression: {e.Message}");
            operationInputField.text = "0";
        }
    }
}
