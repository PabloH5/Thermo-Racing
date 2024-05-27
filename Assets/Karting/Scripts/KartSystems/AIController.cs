using UnityEngine;

namespace KartGame.KartSystems
{
    public class AIController : MonoBehaviour
    {
        public Transform[] waypoints;
        public float speed = 10f;
        public float turnSpeed = 5f;
        public int minReduceSpeed = 7;
        public int minReduceAcc = 4;
        public float waypointThreshold = 1f;
        public bool shouldMove = false;

        private int currentWaypointIndex = 0;
        public ArcadeKartAI arcadeKart;

        void Start()
        {
            arcadeKart = GetComponent<ArcadeKartAI>();

            // Inicialización de waypoints (si es necesario)
            GameObject waypointParent = GameObject.Find("WayPoints");
            if (waypointParent != null)
            {
                waypoints = new Transform[waypointParent.transform.childCount];
                for (int i = 0; i < waypointParent.transform.childCount; i++)
                {
                    waypoints[i] = waypointParent.transform.GetChild(i);
                }
            }
            else
            {
                Debug.LogError("WayPoints GameObject not found.");
            }
        }

        void Update()
        {
            if (!shouldMove || waypoints.Length == 0) return;

            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;

            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget < waypointThreshold)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                arcadeKart.RestoreSpeed(); // Reset the spped and acceleration after pass waypoint
            }

            // Control the kart's movement
            InputData inputData = new InputData
            {
                Accelerate = true,
                Brake = false,
                TurnInput = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up) / 90f
            };

            // Adjust speed for curves
            if (currentWaypointIndex < waypoints.Length - 1)
            {
                Vector3 nextWaypointDirection = (waypoints[currentWaypointIndex + 1].position - waypoints[currentWaypointIndex].position).normalized;
                float angle = Vector3.Angle(directionToTarget, nextWaypointDirection);
                if (angle > 30f) // Adjust threshold angle 
                {
                    arcadeKart.ReduceSpeed(minReduceSpeed, minReduceAcc);
                }
            }
            RotateTowardsTarget(directionToTarget);
            arcadeKart.SetInputData(inputData);
        }

        void RotateTowardsTarget(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }
}
