using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CustomManager;

public class UserKartController : MonoBehaviour
{

    [SerializeField]
    private WheelTypeCustom _wheelType;
    [SerializeField]
    private ChasisType _chasisType;

    void Start()
    {
        // Put the default items if the user doesn't log in.
        if (LoggedUser.UserCode == null)
        {
            ModifyEnumCustom(1, 1);
            SwitchChasisAndWheels();
            return;
        }

        // GET THE INFORMATION FROM CURRENT USER
        UserModel user = UserModel.GetUserById(LoggedUser.UserCode);

        ModifyEnumCustom(user.current_wheels, user.current_chassis);
        SwitchChasisAndWheels();
    }

    private void ModifyEnumCustom(int wheelIndex, int chassisIndex)
    {
        _wheelType = (WheelTypeCustom)wheelIndex;
        _chasisType = (ChasisType)chassisIndex;
    }

    private void SwitchChasisAndWheels()
    {
        transform.Find("DEFAULT CHASIS")?.gameObject.SetActive(false);
        transform.Find("SPIKES CHASIS")?.gameObject.SetActive(false);
        transform.Find("OLD CHASIS")?.gameObject.SetActive(false);
        transform.Find("STAR CHASIS")?.gameObject.SetActive(false);

        Transform selectedChasisTransform = null;
        switch (_chasisType)
        {
            case ChasisType.SpikesC:
                selectedChasisTransform = transform.Find("SPIKES CHASIS");
                selectedChasisTransform?.gameObject.SetActive(true);
                break;

            case ChasisType.OldC:
                selectedChasisTransform = transform.Find("OLD CHASIS");
                selectedChasisTransform?.gameObject.SetActive(true);
                break;

            case ChasisType.StarC:
                selectedChasisTransform = transform.Find("STAR CHASIS");
                selectedChasisTransform?.gameObject.SetActive(true);
                break;

            case ChasisType.DefaultC:
                selectedChasisTransform = transform.Find("DEFAULT CHASIS");
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


}
