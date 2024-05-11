using UnityEngine;
using UnityEngine.EventSystems;
//Script for automatic assignation of accelerate pedal in UI
namespace KartGame.KartSystems
{
    public class AutoAssignEvent : MonoBehaviour
    {
        private KeyboardInput keyboardInput;

        void Start()
        {
            Invoke("ActivateKeyboardInput", 2.0f);
            
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
            keyboardInput.AcccelerateUI();
        }

        void OnPointerUpDelegate(PointerEventData data)
        {
            Debug.Log("Pointer Up");
            keyboardInput.StopAccelerateUI();
        }

        private void ActivateKeyboardInput()
        {
            if (GameObject.Find("KartPlayer") != null)
            {
                keyboardInput = GameObject.Find("KartPlayer").GetComponent<KeyboardInput>();
            }
            else { Debug.Log("KartPlayer Not Found"); }
        }
    }
}