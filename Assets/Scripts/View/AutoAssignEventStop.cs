using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;
//Script for automatic assignation of brake pedal in UI
namespace KartGame.KartSystems
{
    public class AutoAssignEventStop : MonoBehaviour
    {
        private KeyboardInput keyboardInput;

        void Start()
        {
            AssignKeyboardInput();
        }

        void AssignKeyboardInput()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            if (players != null)
            {
                foreach (var playerObject in players)
                {
                    if (AllChildrenActive(playerObject))
                    {
                        keyboardInput = playerObject.GetComponent<KeyboardInput>();
                    }
                }
            }

            EventTrigger trigger = gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
            trigger.triggers.Add(pointerDownEntry);

            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            pointerUpEntry.eventID = EventTriggerType.PointerUp;
            pointerUpEntry.callback.AddListener((data) => { OnPointerUpDelegate((PointerEventData)data); });
            trigger.triggers.Add(pointerUpEntry);
        }

        private bool AllChildrenActive(GameObject obj)
        {
            foreach (Transform child in obj.transform)
            {
                if (!child.gameObject.activeInHierarchy)
                {
                    return false;
                }
            }
            return true;
        }

        void OnPointerDownDelegate(PointerEventData data)
        {
            Debug.Log("Pointer Down");
            keyboardInput.BrakeUI();
        }

        void OnPointerUpDelegate(PointerEventData data)
        {
            Debug.Log("Pointer Up");
            keyboardInput.StopBrakeUI();
        }
    }
}