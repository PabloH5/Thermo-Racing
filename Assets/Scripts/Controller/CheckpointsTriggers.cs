using System;
using KartGame.KartSystems;
using UnityEngine;

public class CheckpointsTriggers : MonoBehaviour
{
    public CheckList CheckpointManager;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            Debug.Log("Get Checkpoint");
            Debug.Log("El carro con el que choco es: " + other.transform.parent.name);
            ArcadeKart kart = other.GetComponentInParent<ArcadeKart>();

            if (kart != null)
            {
                kart.AddOrUpdateCheckpoint(gameObject.name, true, DateTime.Now);
            }
            else
            {
                ArcadeKartSingleplayer kartSingleplayer = other.GetComponentInParent<ArcadeKartSingleplayer>();
                kartSingleplayer.AddOrUpdateCheckpoint(gameObject.name, true, DateTime.Now);
            }
        }
    }
}
