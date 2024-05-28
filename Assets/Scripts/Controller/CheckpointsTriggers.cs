using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsTriggers : MonoBehaviour
{
    public CheckList CheckpointManager;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Get Checkpoint");
            CheckpointManager.ToggleObjects();
        }
    }
}
