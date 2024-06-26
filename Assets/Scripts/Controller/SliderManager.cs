using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class SliderManager : MonoBehaviour
{
    [SerializeField] private Text sliderText;
    [SerializeField] private Slider slider1;
    [SerializeField] private Slider slider2;
    public Slider[] sliders; 
    public Image[] images; 
    public float maxSliderValue = 5.0f;
    public float divisor = 10.0f;
    private Dictionary<int, float> slider1ValueMappings = new Dictionary<int, float>();
    private Dictionary<int, float> slider2ValueMappings = new Dictionary<int, float>();
    public UnityEvent feedbackPositiveEvent;


    void Start()
    {
        InitializeValueMappings();

        slider1.onValueChanged.AddListener(UpdateResultText);
        slider2.onValueChanged.AddListener(UpdateResultText);

        if (sliders.Length > 0 && images.Length == sliders.Length)
        {
            for (int i = 0; i < sliders.Length; i++)
            {
                int index = i; // Capture the current index for use within the listener.
                sliders[i].onValueChanged.AddListener((value) => { UpdateImage(value, index); });
            }
        }

        feedbackPositiveEvent.AddListener(() => {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<ARLLManager>().ActivatePositiveFeedbackGUI();
        });   
    } 

    void InitializeValueMappings()
    {
        // Example mappings for slider1.
        slider1ValueMappings.Add(5, 0.0f); // When slider1 is 0, use 5.0 instead.
        // Add additional mappings for slider1 as needed.
        slider1ValueMappings.Add(4, 0.001f);
        slider1ValueMappings.Add(3, 0.003f);
        slider1ValueMappings.Add(2, 0.005f);
        slider1ValueMappings.Add(1, 0.008f);
        slider1ValueMappings.Add(0, 0.013f);
        // Example mappings for slider2.
        slider2ValueMappings.Add(0, 0.016f); // When slider2 is 0, use 3.0 instead.
        // Add additional mappings for slider2 as needed.
        slider2ValueMappings.Add(1, 0.019f);
        slider2ValueMappings.Add(2, 0.022f);
        slider2ValueMappings.Add(3, 0.025f);
        slider2ValueMappings.Add(4, 0.027f);
        slider2ValueMappings.Add(5, 0.032f);
        // Note: Populate these dictionaries with all 11 mappings for each slider.
    }

    void UpdateImage(float sliderValue, int index)
    {
        images[index].fillAmount = sliderValue / maxSliderValue;
    }

    void UpdateResultText(float value)
    {
        // Call the function to calculate the difference and update the text.
        string result = CalculateDifference();
        sliderText.text = result + " J";

        string pattern = @"992[.,]985 J";
        if (Regex.IsMatch(sliderText.text, pattern))
        {
            Debug.Log("Im in");
            Invoke("InvokeFeedbackPositiveEvent", 0.5f);
        }
    }

    string CalculateDifference()
    {
        float customValue1 = slider1ValueMappings.ContainsKey((int)slider1.value) ? slider1ValueMappings[(int)slider1.value] : 0;
        float customValue2 = slider2ValueMappings.ContainsKey((int)slider2.value) ? slider2ValueMappings[(int)slider2.value] : 0;

        float difference = 0.49f * 101325.0f * (customValue2 - customValue1);

        return difference.ToString();
    }

    private void InvokeFeedbackPositiveEvent()
    {
        feedbackPositiveEvent.Invoke();
    }
}
