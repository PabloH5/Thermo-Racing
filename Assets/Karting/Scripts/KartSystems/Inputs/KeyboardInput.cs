using UnityEngine;
using UnityEngine.UI;

namespace KartGame.KartSystems
{
    public class KeyboardInput : BaseInput
    {
        public string TurnInputName = "Horizontal";
        public string AccelerateButtonName = "Accelerate";
        public string BrakeButtonName = "Brake";

        public Joystick joystick;
        public Button accelerateUIButton;
        public Button brakeUIButton;
        private bool uiAccelerate = false;
        private bool uiBrake = false;

        // void Start()
        // {
        //     if (accelerateUIButton != null)
        //     {
        //         accelerateUIButton.onClick.AddListener(() => { uiAccelerate = true; });
        //     }
        // }
        public override InputData GenerateInput()
        {
            return new InputData
            {
                Accelerate = uiAccelerate,
                Brake = uiBrake,
                TurnInput = joystick.Horizontal
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
