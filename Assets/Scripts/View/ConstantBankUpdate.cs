using UnityEngine;
using UnityEngine.UI;

public class ConstantBankUpdate : MonoBehaviour
{
    [SerializeField]
    private Text massTxt;
    [SerializeField]
    private Text specificHeatPression;
    [SerializeField]
    private Text specificHeat;
    [SerializeField]
    private Text pressionTxt;
    [SerializeField]
    private Text finalVolummeTxt;
    [SerializeField]
    private Text finalTemperatureTxt;
    [SerializeField]
    private Text eTxt;
    [SerializeField]
    private Text deltaETxt;

    public void UpdateMass(float value)
    {
        massTxt.text = value + " kg";
    }
    public void UpdateSpecificHeatPression(float value)
    {
        specificHeatPression.text = value + " J/(kg·°C)";
    }
    public void UpdateSpecificHeat(float value)
    {
        specificHeat.text = value + " J/(kg·°C)";
    }
    public void UpdatePression(float value)
    {
        pressionTxt.text = value + " atm";
    }
    public void UpdateVolumme(float value)
    {
        finalVolummeTxt.text = value + " m³";
    }
    public void UpdateFinalTemperature(float value)
    {
        finalTemperatureTxt.text = value + " °C";
    }
    public void UpdateFinalTemperatureWithString()
    {
        finalTemperatureTxt.text += " --> 353.15K";
    }
    public void UpdateE(float value)
    {
        eTxt.text = value + " %";
    }
    public void UpdateDeltaE(float value)
    {
        deltaETxt.text = value + " J";
    }
}
