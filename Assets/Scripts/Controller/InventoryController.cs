using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<WheelModel> userInventory = InventoryModel.GetPossibleWheelsRewards(2222222);

        userInventory.ForEach(x => Debug.Log(x.wheel_name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
