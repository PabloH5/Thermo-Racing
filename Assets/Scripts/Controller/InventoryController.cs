using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private TMP_Text userInventoryText;

    [SerializeField] private TMP_Text awardsText;



    // Start is called before the first frame update
    void Start()
    {
        List<InventoryUser> userInventory = InventoryUser.GetUserInventory(2222222);
        List<WheelModel> userAwardsAvailable = InventoryRawModel.GetPossibleWheelsRewards(2222222);

        //userInventory.ForEach(inventory => userInventoryText.text += $"- ID {(inventory.wheel_id !=null ? (inventory.wheel_id) : (inventory.chassis_id))} - Nombre {(inventory.wheel_id !=null ? inventory.wheel_name : inventory.chassis_name)} \n");

        userInventory.ForEach(item => userInventoryText.text += $"ITEM ID {item.item_id} - {item.item_name} - {item.item_type} \n");

        userAwardsAvailable.ForEach(x =>awardsText.text +=$"- Item {x.wheel_name} | WheelId {x.wheel_id} \n");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
