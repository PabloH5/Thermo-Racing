using UnityEngine;
using UnityEngine.Assertions;

namespace KartGame.KartSystems
{
    public class KartPlayerAnimator : MonoBehaviour
    {
        public Animator PlayerAnimator;
        public ArcadeKart KartMltiplayer;
        public ArcadeKartSingleplayer kartSingle;

        public string SteeringParam = "Steering";
        public string GroundedParam = "Grounded";

        int m_SteerHash, m_GroundHash;

        float steeringSmoother;

        void Awake()
        {

            // Assert.IsNotNull(KartMltiplayer, "No ArcadeKart found!");
            // Assert.IsNotNull(kartSingle, "No ArcadeKart Single found!");
            // Assert.IsNotNull(PlayerAnimator, "No PlayerAnimator found!");
            m_SteerHash = Animator.StringToHash(SteeringParam);
            m_GroundHash = Animator.StringToHash(GroundedParam);
        }

        void Update()
        {
            if (KartMltiplayer != null)
            {
                steeringSmoother = Mathf.Lerp(steeringSmoother, KartMltiplayer.Input.TurnInput, Time.deltaTime * 5f);
                PlayerAnimator.SetFloat(m_SteerHash, steeringSmoother);
                PlayerAnimator.SetBool(m_GroundHash, KartMltiplayer.GroundPercent >= 0.5f);
            }
            else
            {
                steeringSmoother = Mathf.Lerp(steeringSmoother, kartSingle.Input.TurnInput, Time.deltaTime * 5f);
                PlayerAnimator.SetFloat(m_SteerHash, steeringSmoother);
                PlayerAnimator.SetBool(m_GroundHash, kartSingle.GroundPercent >= 0.5f);
            }
        }
    }
}
