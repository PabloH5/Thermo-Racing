using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWheelsController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private DragAndDropUI dragAndDropUI;
    [SerializeField]
    private TiresDataBase wheelsData;

    [Header("Put here data for Inflate the wheel section")]
    [SerializeField]
    private GameObject thermometerInflate;
    private ThermometerWheelsView thInflate;
    [SerializeField]
    private List<float> listFramesThermometer = new List<float>();
    [SerializeField]
    private float tolerance;

    void Start()
    {
        thInflate = thermometerInflate.GetComponent<ThermometerWheelsView>();
    }

    public float WheelWeigh(int i)
    {
        float weight = wheelsData.tires[i].weight;
        return weight;
    }

    public void ToInitStateWheel()
    {
        dragAndDropUI.ToInitialState();
    }
    public void SwitchSpriteTH(int touchCount)
    {
        thInflate.SwitchSprite(touchCount);
    }
    public void DecreaseThermometer(int touchCount)
    {
        thInflate.SwitchSprite(touchCount);
    }

    public void CompareState(int touchCount)
    {
        float currentFrame = thInflate.CurrentFrame();
        // float targetFrame;
        // tolerance = 0.05f;
        if (touchCount == 0)
        {
            thInflate.SwitchSpeed(0);
            thInflate.Jump2Time(0);
        }
        else if (touchCount < 10)
        {
            // ChangeState(0, 1);
            thInflate.SwitchSpeed(0.8f);
            StopAnimation(touchCount);
        }
        else if (touchCount > 10 && touchCount < 20)
        {
            thInflate.SwitchSpeed(0.8f);
            StopAnimation(touchCount);
        }
        else if (touchCount >= 20 && touchCount < 30)
        {
            thInflate.SwitchSpeed(0.8f);
            StopAnimation(touchCount);
        }
        else if (touchCount >= 30)
        {
            Debug.Log("RESPLANDOR Y HACE BUUUUUUM");
        }
    }
    public void StopAnimation(int touchCount)
    {
        float targetFrame;
        if (touchCount == 0)
        {
            thInflate.SwitchSpeed(0);
            thInflate.Jump2Time(0);
        }
        else if (touchCount < 10)
        {
            targetFrame = listFramesThermometer[3];
            if (thInflate.CurrentFrame() >= targetFrame - tolerance && thInflate.CurrentFrame() <= targetFrame + tolerance)
            {
                // ChangeState(4, 1);
                thInflate.SwitchSpeed(0);
                Debug.Log("Pause in 10");
            }
        }
        else if (touchCount > 10 && touchCount < 20)
        {
            targetFrame = listFramesThermometer[6];
            if (thInflate.CurrentFrame() == targetFrame - tolerance && thInflate.CurrentFrame() <= targetFrame + tolerance)
            {
                // ChangeState(8, 1);
                thInflate.SwitchSpeed(0);
                Debug.Log("Pause in 20");
            }
        }
        else if (touchCount >= 20 && touchCount < 30)
        {
            if (touchCount == 21)
            {
                targetFrame = listFramesThermometer[7];
                if (thInflate.CurrentFrame() == targetFrame - tolerance && thInflate.CurrentFrame() <= targetFrame + tolerance)
                {
                    // ChangeState(8, 1);
                    thInflate.SwitchSpeed(0);
                    Debug.Log("Pause in 21");
                }
            }
            else
            {
                targetFrame = listFramesThermometer[10];
                if (thInflate.CurrentFrame() == targetFrame - tolerance && thInflate.CurrentFrame() <= targetFrame + tolerance)
                {
                    // ChangeState(8, 1);
                    thInflate.SwitchSpeed(0);
                    Debug.Log("Pause in 30");
                }
            }
        }
    }
}
