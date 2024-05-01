using UnityEngine;

namespace KartGame.KartSystems
{

    public class KeyboardInput : BaseInput
    {
        public string TurnInputName = "Horizontal";
        public string AccelerateButtonName = "Accelerate";
        public string BrakeButtonName = "Brake";

        public Joystick joystick;
        public override InputData GenerateInput()
        {
            float turn = Input.GetAxis("Horizontal");
            Debug.Log("Turn Input: " + turn);
            return new InputData
            {
                Accelerate = Input.GetButton(AccelerateButtonName),
                Brake = Input.GetButton(BrakeButtonName),
                TurnInput = joystick.Horizontal
            };
        }
    }
}
