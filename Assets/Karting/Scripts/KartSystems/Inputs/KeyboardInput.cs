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
        private ArcadeKart arcadeKart;

        void Awake()
        {
            arcadeKart = GetComponent<ArcadeKart>();
            if (GameObject.Find("FixedJoystick") != null)
            {
                joystick = GameObject.Find("FixedJoystick").GetComponent<Joystick>();
            }
            else { Debug.Log("Controllers Not founded"); }
        }

        public override InputData GenerateInput()
        {
            if (arcadeKart != null && !arcadeKart.IsOwner) {
                return new InputData(); 
            }

            float turnInput = joystick != null ? joystick.Horizontal : Input.GetAxis(TurnInputName);
            bool accelerate = uiAccelerate || Input.GetButton(AccelerateButtonName);
            bool brake = uiBrake || Input.GetButton(BrakeButtonName);

            return new InputData {
                Accelerate = accelerate,
                Brake = brake,
                TurnInput = turnInput
            };
        }

        public void AccelerateUI()
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
