using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserRaceInformation : MonoBehaviour
{
    public int numberOfLaps { get; set; }

    void Start()
    {
        // In the first moment, the user has to touch the goal line at least once before start to count laps.
        numberOfLaps = -1;
    }
}
