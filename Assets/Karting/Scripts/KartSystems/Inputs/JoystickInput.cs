using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : MonoBehaviour
{
    public Joystick joystick;
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float sideways = Input.GetAxis("Sideways");

        // Imprime los valores en la consola para verificar
        // Debug.Log("Horizontal: " + joystick.Horizontal + ", Vertical: " + vertical + ", Sideways: " + sideways);
    }
}
