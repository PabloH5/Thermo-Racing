using System;
using UnityEngine;

namespace KartGame.KartSystems
{
    public class KartTest : MonoBehaviour
    {
        [System.Serializable]
        public struct Stats
        {
            public float TopSpeed;
            public float Acceleration;
            public float ReverseSpeed;
            public float ReverseAcceleration;
            public float Braking;
            public float CoastingDrag;
            public float Steer;
            [Range(0.2f, 1)]
            public float AccelerationCurve;
            [Range(0.0f, 1.0f)]
            public float Grip;
            public float AdditionalTurnTorque;

            public static Stats operator +(Stats a, Stats b)
            {
                return new Stats
                {
                    Acceleration = a.Acceleration + b.Acceleration,
                    Braking = a.Braking + b.Braking,
                    CoastingDrag = a.CoastingDrag + b.CoastingDrag,
                    ReverseAcceleration = a.ReverseAcceleration + b.ReverseAcceleration,
                    ReverseSpeed = a.ReverseSpeed + b.ReverseSpeed,
                    TopSpeed = a.TopSpeed + b.TopSpeed,
                    Steer = a.Steer + b.Steer,
                    AccelerationCurve = a.AccelerationCurve + b.AccelerationCurve,
                    Grip = a.Grip + b.Grip,
                };
            }
        }
        [Header("Vehicle Physics")]
        public Transform CenterOfMass;
        [Range(0.0f, 20.0f)]
        public float AirborneReorientationCoefficient = 1.0f;
        public Rigidbody Rigidbody { get; private set; }
        public Stats baseStats = new Stats
        {
            TopSpeed = 10f,
            Acceleration = 5f,
            ReverseAcceleration = 5f,
            ReverseSpeed = 5f,
            Steer = 5f,
            Braking = 10f,
            CoastingDrag = 4f,
            AccelerationCurve = 0.5f,
            Grip = 0.95f
        };

        private Stats m_FinalStats;
        private bool m_CanMove = true;

        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            m_FinalStats = baseStats;
            if (CenterOfMass)
            {
                Rigidbody.centerOfMass = CenterOfMass.localPosition;
            }
        }

        void FixedUpdate()
        {
            if (m_CanMove)
            {
                MoveVehicle();
                AdjustPhysics();
            }
        }

        void MoveVehicle()
        {
            float moveInput = Input.GetAxis("Vertical");
            float turnInput = Input.GetAxis("Horizontal");
            bool isBraking = moveInput < 0;

            float speedFactor = isBraking ? m_FinalStats.ReverseAcceleration : m_FinalStats.Acceleration;
            float movement = moveInput * speedFactor;
            Rigidbody.AddForce(transform.forward * movement * m_FinalStats.AccelerationCurve, ForceMode.Acceleration);

            float turning = turnInput * m_FinalStats.Steer;
            // Añadir un factor adicional basado en el input de giro
            turning += turnInput * m_FinalStats.AdditionalTurnTorque;
            Rigidbody.AddTorque(transform.up * turning, ForceMode.VelocityChange);
        }
        void AdjustPhysics()
        {
            // Aquí ajustamos el centro de masa cada frame, en caso de que sea necesario.
            if (CenterOfMass)
            {
                Rigidbody.centerOfMass = CenterOfMass.localPosition;
            }

            // Aquí puedes añadir cualquier otra lógica relacionada con la física, como la reorientación en el aire.
            if (!IsGrounded()) // Asumiendo que tienes una función IsGrounded que determina si el kart está en el suelo
            {
                // Aplicar un coeficiente de reorientación en el aire para controlar cómo el kart se ajusta en el aire
                Vector3 predictedUp = Quaternion.AngleAxis(
                    Rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * AirborneReorientationCoefficient / Rigidbody.mass,
                    Rigidbody.angularVelocity
                ) * transform.up;

                Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
                Rigidbody.AddTorque(torqueVector * AirborneReorientationCoefficient * AirborneReorientationCoefficient);
            }
        }

        private bool IsGrounded()
        {
            // Implementa tu lógica para determinar si el kart está en el suelo
            return false;
        }
    }
}
