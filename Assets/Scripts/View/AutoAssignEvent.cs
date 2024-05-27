using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;

namespace KartGame.KartSystems
{
    public class AutoAssignEvent : MonoBehaviour
    {
        private KeyboardInput keyboardInput;

        void Start()
        {
            if (RaceMultiplayerController.playMultiplayer) {
            AssignKeyboardInput();
            }
            else {
                Debug.Log("Im trying a panguanada");
                if (GameObject.Find("KartPlayer") != null)
                {
                    keyboardInput = GameObject.Find("KartPlayer").GetComponent<KeyboardInput>();
                }
                else { Debug.Log("KartPlayer Not Found"); }
            }
        }

        public void AssignKeyboardInput()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            Debug.Log(players.ToString());

            if (players != null)
            {
                foreach (var playerObject in players)
                {
                    if (AllChildrenActive(playerObject))
                    {
                        keyboardInput = playerObject.GetComponent<KeyboardInput>();
                        Debug.Log("Im KeyboardInput in AutoAssignEvent " + keyboardInput);
                        if (keyboardInput != null)
                        {
                            Debug.Log("I found KeyboardInput");
                            break;
                        }
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
            if (keyboardInput != null)
            {
                keyboardInput.AccelerateUI();
            }
            else
            {
                Debug.LogError("keyboardInput is null. Cannot call AccelerateUI.");
            }
        }

        void OnPointerUpDelegate(PointerEventData data)
        {
            Debug.Log("Pointer Up");
            if (keyboardInput != null)
            {
                keyboardInput.StopAccelerateUI();
            }
            else
            {
                Debug.LogError("keyboardInput is null. Cannot call StopAccelerateUI.");
            }
        }
    }
}
