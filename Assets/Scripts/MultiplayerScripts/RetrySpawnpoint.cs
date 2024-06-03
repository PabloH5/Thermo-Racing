using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using Unity.Netcode;
using UnityEngine;

public class RetrySpawnpoint : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "KartPlayer")
        {
            NetworkObject networkObject = GetComponent<NetworkObject>();

            if (RaceMultiplayerController.playMultiplayer && networkObject.IsOwner)
            {
                Debug.Log("Im going to move to my spawnpoint");
                other.GetComponent<ArcadeKart>().UpdateMySpawn();
                return;
            }
            else {
                Debug.Log("I am not the owner");
                return;
            }
        }
    }

}
