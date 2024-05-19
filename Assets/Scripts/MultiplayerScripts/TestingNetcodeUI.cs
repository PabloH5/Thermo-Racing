using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private GameObject cameraUI;
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;   
        
    void Start() {
        startHostButton.onClick.AddListener(StartHost);
        startClientButton.onClick.AddListener(StartClient);
    }
    
    void StartHost() 
    {
        RaceMultiplayerController.Instance.StartHost();
        Hide();
    }

    void StartClient() 
    {
        RaceMultiplayerController.Instance.StartClient();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        cameraUI.SetActive(false);
    }
}
