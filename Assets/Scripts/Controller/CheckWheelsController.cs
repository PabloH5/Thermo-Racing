using UnityEngine;

public class CheckWheelsController : MonoBehaviour
{

    private Tires wheelData;
    [Tooltip("Put here the Constant Banck Controller")]
    [SerializeField]
    private ConstantBankUpdate constantBank;

    [Header("Put here de data for Wheigh the wheel section")]
    [Header("Canvas Weigh The Wheel")]
    [Tooltip("Here put the element with Drag and Drop Script (Wheel)")]
    [SerializeField]
    private DragAndDropUI dragAndDropUI;

    [Tooltip("Here put the element with tires Manager Script (Wheel)")]
    [SerializeField]
    private TiresManager tireManager;
    [Header("Canvas (Select Type)")]
    [Tooltip("Here put the description above the scale")]
    [SerializeField]
    private GameObject descriptionScale;
    [Tooltip("Here put the arrow for change to the next wheel")]
    [SerializeField]
    private GameObject arrowNext;
    [Tooltip("Here put the arrow for change to the back wheel")]
    [SerializeField]
    private GameObject arrowBack;

    [Header("Put here data for Inflate the wheel section")]
    [Header("Inflate Wheel")]
    [Tooltip("Put here the thermometer of Inflate Wheel (Thermometer 1)")]
    [SerializeField]
    private GameObject thermometerInflate;
    private ThermometerWheelsView thInflate;

    [Tooltip("Put here the Wheel (Wheel)")]
    [SerializeField]
    private WheelToInflate wheelInflate;
    [Tooltip("Put here Positive FeedBack")]

    void Start()
    {
        thInflate = thermometerInflate.GetComponent<ThermometerWheelsView>();
    }
    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }
    public Tires WheelData
    {
        get { return wheelData; }
        set { wheelData = value; }
    }
    public float WheelWeigh(float i)
    {
        float weight = WheelData.weight;
        float specificHeat = WheelData.specificHeat;
        constantBank.UpdateMass(weight);
        constantBank.UpdateSpecificHeatPression(specificHeat);
        return weight;
    }
    public void HideArrows()
    {
        arrowNext.SetActive(false);
        arrowBack.SetActive(false);
        descriptionScale.SetActive(false);
    }
    public void ToInitStateWheel()
    {
        dragAndDropUI.ToInitialState();
        arrowNext.SetActive(true);
        arrowBack.SetActive(true);
        descriptionScale.SetActive(true);
    }
    public void SwitchSpriteTH(int touchCount)
    {
        if (touchCount > -1 && touchCount < 30)
        {
            thInflate.SwitchSprite(touchCount);
        }
        else
        {
            Debug.Log("KABOM");
        }

        // Debug.Log(touchCount);
    }

    // This method controll the change of wheel sprites depend of thr touchcount and active the feedback depend of the time
    public void SwitchSpriteWH(int touchCount, float time)
    {
        if (touchCount <= 5)
        {
            wheelInflate.SwitchSprite(0);
        }
        else if (touchCount > 5 && touchCount < 7)
        {
            wheelInflate.SwitchSprite(1);
        }
        else if (touchCount > 7 && touchCount < 12)
        {
            wheelInflate.SwitchSprite(2);
        }
        else if (touchCount >= 12 && touchCount < 23)
        {
            wheelInflate.SwitchSprite(3);
        }
        else if (touchCount > 24 && touchCount <= 29)
        {
            wheelInflate.SwitchSprite(4);
            //!HECTOR METE TU  EXPLOSION AQUI
        }
        else { Debug.Log("KABOOOM"); }
    }
    // public void ActivePositiveFB(int touchCount)
    // {
    //     positiveFeedBackIF.SetActive(true);
    //     //!PUT A REAL VALUE FOR PRESSION
    //     constantBank.UpdatePression(0.49f);
    //     //!PUT A REAL VALUE FOR FINAL VOLUMME
    //     constantBank.UpdateVolumme(0.025f);
    //     PauseGame();
    //     thInflate.SwitchSprite(touchCount);
    // }
    // public void ActiveNegativeFB(int touchCount)
    // {
    //     negativeFeedBackIF.SetActive(true);
    //     PauseGame();
    //     thInflate.SwitchSprite(touchCount);
    // }
}
