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

        public Rigidbody rigidbody;

        void Awake()
        {
            arcadeKart = GetComponent<ArcadeKart>();
            rigidbody = GetComponent<Rigidbody>();
            if (GameObject.Find("FixedJoystick") != null)
            {
                joystick = GameObject.Find("FixedJoystick").GetComponent<Joystick>();
            }
            else { Debug.Log("Controllers Not founded"); }
        }

        void Update()
        {
            float joyStickValue = joystick != null ? joystick.Horizontal : Input.GetAxis(TurnInputName);
            if (joyStickValue != 0)
            {
                rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            }
            else
            {
                rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        }

        public override InputData GenerateInput()
        {
            if (arcadeKart != null && !arcadeKart.IsOwner)
            {
                return new InputData();
            }

            float turnInput = joystick != null ? joystick.Horizontal : Input.GetAxis(TurnInputName);
            bool accelerate = uiAccelerate || Input.GetButton(AccelerateButtonName);
            bool brake = uiBrake || Input.GetButton(BrakeButtonName);


            return new InputData
            {
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