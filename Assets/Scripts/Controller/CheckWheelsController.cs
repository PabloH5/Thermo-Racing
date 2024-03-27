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
        Debug.Log(touchCount);
    }
    public void SwitchSpriteWH(int touchCount)
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
            Debug.Log("BUUUUUUUUUUUUUM");
        }
    }
}
