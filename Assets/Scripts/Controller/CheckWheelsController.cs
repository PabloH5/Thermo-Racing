using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckWheelsController : MonoBehaviour
{
    private Tires wheelData;
    [SerializeField]
    private DragAndDropUI dragAndDropUI;
    [SerializeField]
    private TiresManager tireManager;
    [SerializeField]
    private GameObject arrowNext;
    [SerializeField]
    private GameObject arrowBack;

    [Header("Put here data for Inflate the wheel section")]
    [SerializeField]
    private GameObject thermometerInflate;
    private ThermometerWheelsView thInflate;
    [SerializeField]
    private WheelToInflate wheelInflate;
    [SerializeField]
    private GameObject positiveFeedBackIF;
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
