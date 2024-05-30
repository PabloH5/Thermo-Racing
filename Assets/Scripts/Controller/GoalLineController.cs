using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLineController : MonoBehaviour
{

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerCollider")
        {
            Debug.Log(other.gameObject.name);
            GameObject kartPlayerParent = other.transform.parent.gameObject;
            GoalQTEController goalQTEController = kartPlayerParent.GetComponentInChildren<GoalQTEController>();    
            goalQTEController.ManageLapBehaviour(goalQTEController.gameObject);
        }
    }
}
