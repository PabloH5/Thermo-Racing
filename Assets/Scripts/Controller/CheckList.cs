using System;
using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;

public class CheckList : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectList = new List<GameObject>();

    public List<GameObject> GetObjectList()
    {
        return objectList;
    }

    public void PrintObjectList()
    {
        foreach (GameObject obj in objectList)
        {
            Debug.Log("Objeto en la lista: " + obj.name);
        }
    }

    public void InitializeChekpointArcadeKart(GameObject kartPlayer, bool _IsMultiplayer)
    {
        if (_IsMultiplayer)
        {
            Debug.Log("Im initializing checkpoints in multiplayer");
            foreach (GameObject checkpoint in objectList)
            {
                ArcadeKart arcadeKart = kartPlayer.GetComponent<ArcadeKart>();
                arcadeKart.AddOrUpdateCheckpoint(checkpoint.name, false, DateTime.Now); 
            }
        }
        else 
        {
            Debug.Log("Im initializing checkpoints in singleplayer");
            foreach (GameObject checkpoint in objectList)
            {
                ArcadeKartSingleplayer arcadeKartSingleplayer = kartPlayer.GetComponent<ArcadeKartSingleplayer>();
                arcadeKartSingleplayer.AddOrUpdateCheckpoint(checkpoint.name, false, DateTime.Now); 
            }
        }

    }
}
