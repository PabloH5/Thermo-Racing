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
    private WheelToInflate wheelInflate;
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
    public void SwitchSpriteWH(int touchCount)
    {
        if (touchCount <= 10)
        {
            wheelInflate.SwitchSprite(0);
        }
        else if (touchCount > 10 && touchCount < 15)
        {
            wheelInflate.SwitchSprite(1);
        }
        else if (touchCount > 15 && touchCount < 20)
        {
            wheelInflate.SwitchSprite(2);
        }
        else if (touchCount >= 21 && touchCount < 24)
        {
            wheelInflate.SwitchSprite(3);
        }
        else if (touchCount > 24 && touchCount <= 30)
        {
            wheelInflate.SwitchSprite(4);
            Debug.Log("BUUUUUUUUUUUUUM");
        }

    }
}
