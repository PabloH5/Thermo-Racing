using UnityEngine;
using UnityEngine.UI;

namespace KartGame.KartSystems
{
    public class KeyboardInput : BaseInput
    {
        public string TurnInputName = "Horizontal";
        public string AccelerateButtonName = "Accelerate";
        public string BrakeButtonName = "Brake";
        private Joystick joystick;
        private bool uiAccelerate = false;
        private bool uiBrake = false;

        void Awake()
        {
            if (GameObject.Find("FixedJoystick") != null)
            {
                joystick = GameObject.Find("FixedJoystick").GetComponent<Joystick>();
            }
            else { Debug.Log("Controllers Not founded"); }
        }
        public override InputData GenerateInput()
        {
            float turnInput = joystick != null ? joystick.Horizontal : 0f;
            return new InputData
            {
                Accelerate = uiAccelerate,
                Brake = uiBrake,
                TurnInput = turnInput
            };
        }

        public void AcccelerateUI()
        {
            uiAccelerate = true;
        }
        public void StopAccelerateUI()
        {
            uiAccelerate = false;
        }
        public void BrakeUI()
        {
            uiBrake = true;
        }
        public void StopBrakeUI()
        {
            uiBrake = false;
        }
    }
}
