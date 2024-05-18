using UnityEngine;
using Unity.Netcode;

public class OwnerCheck : NetworkBehaviour
{
    void Start()
    {
        Debug.Log(gameObject.name + " - IsOwner en Start: " + IsOwner);
    }

    void Update()
    {
        Debug.Log(gameObject.name + " - IsOwner en Update: " + IsOwner);
    }
}