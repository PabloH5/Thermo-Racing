using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomManager : MonoBehaviour
{
    public enum WheelTypeCustom
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
    private WheelTypeCustom _wheelType;
    [SerializeField]
    private ChasisType _chasisType;
    public GameObject kartPlayer;


    [SerializeField] private GameObject wheelContainer;
    [SerializeField] private GameObject chassisContainer;


    [SerializeField] private GameObject feedbackCustomizationModal;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Customization")
        {
            LoadUserInventory();
        }

        if (GameObject.Find("KartPlayer") != null)
        {
            kartPlayer = GameObject.Find("KartPlayer");
        }
        else { Debug.LogWarning("KartPlayer Not Found"); }
    }

    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Track1Core" || SceneManager.GetActiveScene().name == "Track2")
        {
            return;
        }
        SwitchChasisAndWheels();
    }
    public void SetWheelType(int wheelTypeIndex)
    {
        _wheelType = (WheelTypeCustom)wheelTypeIndex;
    }

    public void SetChasisType(int chasisTypeIndex)
    {
        _chasisType = (ChasisType)chasisTypeIndex;
    }

    public void SwitchKartParts()
    {
        SwitchChasisAndWheels();
        Debug.Log("----------------------------------------------------");
        Debug.Log(kartPlayer);
        Debug.Log(kartPlayer.name);
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
                    case WheelTypeCustom.SpikesW:
                        wheelsParent.Find("SPIKES WHEELS")?.gameObject.SetActive(true);
                        break;

                    case WheelTypeCustom.OldW:
                        wheelsParent.Find("OLD WHEELS")?.gameObject.SetActive(true);
                        break;

                    case WheelTypeCustom.StarW:
                        wheelsParent.Find("STAR WHEELS")?.gameObject.SetActive(true);
                        break;

                    case WheelTypeCustom.DefaultW:
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
        UserModel.UpdateCurrentGoKart((int)_wheelType, (int)_chasisType, LoggedUser.UserCode);
        feedbackCustomizationModal.SetActive(true);
    }

    public void HideFeedbackModal()
    {
        feedbackCustomizationModal.SetActive(false);
    }

    public void ModifyEnumCustom(int wheelIndex, int chassisIndex)
    {
        _wheelType = (WheelTypeCustom)wheelIndex;
        _chasisType = (ChasisType)chassisIndex;
    }

    public void LoadUserInventory()
    {
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
