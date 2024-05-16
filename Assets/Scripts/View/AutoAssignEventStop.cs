using UnityEngine;
using UnityEngine.EventSystems;
//Script for automatic assignation of brake pedal in UI
namespace KartGame.KartSystems
{
    public class AutoAssignEventStop : MonoBehaviour
    {
        private KeyboardInput keyboardInput;
        void Awake()
        {
            if (GameObject.Find("KartPlayer") != null)
            {
                keyboardInput = GameObject.Find("KartPlayer").GetComponent<KeyboardInput>();
            }
            else { Debug.Log("KartPlayer Not Found"); }
        }
        void Start()
        {
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