using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomManager : MonoBehaviour
{
    public enum WheelType
    {
        DefaultW = 1,
        SpikesW = 2,
        OldW = 3,
        StarW = 4,
    }

    public enum ChasisType
    {
        DefaultC = 1,
        SpikesC = 2,
        OldC = 3,
        StarC = 4,
    }

    [SerializeField]
    private WheelType _wheelType;
    [SerializeField]
    private ChasisType _chasisType;
    public GameObject kartPlayer;


    [SerializeField] private GameObject wheelContainer;
    [SerializeField] private GameObject chassisContainer;

    void Start()
    {
        LoadUserInventory();


        if (GameObject.Find("KartPlayer4Custom") != null)
        {
            kartPlayer = GameObject.Find("KartPlayer4Custom");
        }
        else { Debug.LogWarning("KartPlayer4Custom Not Found"); }
    }
    void FixedUpdate()
    {
        SwitchChasisAndWheels();
    }
    public void SetWheelType(int wheelTypeIndex)
    {
        _wheelType = (WheelType)wheelTypeIndex;
    }

    public void SetChasisType(int chasisTypeIndex)
    {
        _chasisType = (ChasisType)chasisTypeIndex;
    }
    private void SwitchChasisAndWheels()
    {
        kartPlayer.transform.Find("DEFAULT CHASIS")?.gameObject.SetActive(false);
        kartPlayer.transform.Find("SPIKES CHASIS")?.gameObject.SetActive(false);
        kartPlayer.transform.Find("OLD CHASIS")?.gameObject.SetActive(false);
        kartPlayer.transform.Find("STAR CHASIS")?.gameObject.SetActive(false);

        Transform selectedChasisTransform = null;
        switch (_chasisType)
        {
            case ChasisType.SpikesC:
                selectedChasisTransform = kartPlayer.transform.Find("SPIKES CHASIS");
                selectedChasisTransform?.gameObject.SetActive(true);
                break;

            case ChasisType.OldC:
                selectedChasisTransform = kartPlayer.transform.Find("OLD CHASIS");
                selectedChasisTransform?.gameObject.SetActive(true);
                break;

            case ChasisType.StarC:
                selectedChasisTransform = kartPlayer.transform.Find("STAR CHASIS");
                selectedChasisTransform?.gameObject.SetActive(true);
                break;

            case ChasisType.DefaultC:
                selectedChasisTransform = kartPlayer.transform.Find("DEFAULT CHASIS");
                selectedChasisTransform?.gameObject.SetActive(true);
                break;

            default:
                break;
        }

        if (selectedChasisTransform != null)
        {
            Transform wheelsParent = selectedChasisTransform.Find("Wheels");
            if (wheelsParent != null)
            {
                wheelsParent.Find("SPIKES WHEELS")?.gameObject.SetActive(false);
                wheelsParent.Find("OLD WHEELS")?.gameObject.SetActive(false);
                wheelsParent.Find("STAR WHEELS")?.gameObject.SetActive(false);
                wheelsParent.Find("DEFAULT WHEELS")?.gameObject.SetActive(false);
                switch (_wheelType)
                {
                    case WheelType.SpikesW:
                        wheelsParent.Find("SPIKES WHEELS")?.gameObject.SetActive(true);
                        break;

                    case WheelType.OldW:
                        wheelsParent.Find("OLD WHEELS")?.gameObject.SetActive(true);
                        break;

                    case WheelType.StarW:
                        wheelsParent.Find("STAR WHEELS")?.gameObject.SetActive(true);
                        break;

                    case WheelType.DefaultW:
                        wheelsParent.Find("DEFAULT WHEELS")?.gameObject.SetActive(true);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public void ChangeCurrentGoKartUser()
    {
        // Update current register from DB with user '2222222'
        // TODO: use the user id from loggedUser class
        Debug.Log(LoggedUser.Username);
        UserModel.UpdateCurrentGoKart((int)_wheelType, (int)_chasisType, "2222222");
    }

    public void ModifyEnumCustom(int wheelIndex, int chassisIndex)
    {
        _wheelType = (WheelType)wheelIndex;
        _chasisType = (ChasisType)chassisIndex;
    }

    public void LoadUserInventory()
    {
        //---------------------------------------
        // LOGGED USER HERE IS TEMPORARY
        LoggedUser.LogInUser("2222222", "test");
        // ------------------------------------------------



        // BRING THE USER INVENTORY FROM DB
        List<InventoryUser> userInventory = InventoryUser.GetUserInventory(LoggedUser.UserCode);
        Transform foo;
        foreach (var item in userInventory)
        {
            
            switch (item.item_type)
            {
                
                case "WHEEL":
                    foo = wheelContainer.gameObject.transform.GetChild(item.item_id - 1);
                    foo.Find("LockedBG").gameObject.SetActive(false);
                    break;
                case "CHASSIS":
                    foo = chassisContainer.gameObject.transform.GetChild(item.item_id - 1);
                    foo.Find("LockedBG").gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
        userInventory.ForEach(item => Debug.Log($"ID {item.item_id} - {item.item_name} - {item.item_type}"));
    }
}
