using UnityEngine;

public class CheckWheelsController : MonoBehaviour
{

    private Tires wheelData;

    [Header("Put here de data for Wheigh the wheel section")]
    [Header("Canvas Weigh The Wheel")]
    [Tooltip("Here put the element with Drag and Drop Script (Wheel)")]
    [SerializeField]
    private DragAndDropUI dragAndDropUI;

    [Tooltip("Here put the element with tires Manager Script (Wheel)")]
    [SerializeField]
    private TiresManager tireManager;
    [Header("Canvas (Select Type)")]
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
    [SerializeField]
    private GameObject positiveFeedBackIF;
    [Tooltip("Put here Negative FeedBack")]
    [SerializeField]
    private GameObject negativeFeedBackIF;

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
        return weight;
    }
    public void HideArrows()
    {
        arrowNext.SetActive(false);
        arrowBack.SetActive(false);
    }
    public void ToInitStateWheel()
    {
        dragAndDropUI.ToInitialState();
        arrowNext.SetActive(true);
        arrowBack.SetActive(true);
    }
    public void SwitchSpriteTH(int touchCount)
    {
        thInflate.SwitchSprite(touchCount);
        Debug.Log(touchCount);
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
        else if (touchCount >= 12 && touchCount < 18)
        {
            wheelInflate.SwitchSprite(3);
        }
        else if (touchCount > 18 && touchCount <= 40)
        {
            wheelInflate.SwitchSprite(4);
        }
    }
    public void ActivePositiveFB(int touchCount)
    {
        positiveFeedBackIF.SetActive(true);
        PauseGame();
        thInflate.SwitchSprite(touchCount);
    }
    public void ActiveNegativeFB(int touchCount)
    {
        negativeFeedBackIF.SetActive(true);
        PauseGame();
        thInflate.SwitchSprite(touchCount);
    }
}
