using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWheelsController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private DragAndDropUI dragAndDropUI;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float WheelWeigh()
    {
        float weight = 10;
        return weight;
    }

    public void ToInitStateWheel()
    {
        dragAndDropUI.ToInitialState();
    }
}
