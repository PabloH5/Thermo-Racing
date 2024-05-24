using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanUP : MonoBehaviour
{

    void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (RaceMultiplayerController.Instance != null)
        {
            Destroy(RaceMultiplayerController.Instance.gameObject);
        }
    }

}
