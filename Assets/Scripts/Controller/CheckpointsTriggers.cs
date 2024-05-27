using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsTriggers : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            Debug.Log("Get Checkpoint");
        }
    }
}
