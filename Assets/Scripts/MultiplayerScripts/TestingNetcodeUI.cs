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
    
    void StartHost() {
        Debug.Log("Starting host");
        NetworkManager.Singleton.StartHost();
        RaceController.Instance.LocalPlayerIsReady();
        Hide();
    }

    void StartClient() {
        Debug.Log("Starting client");
        NetworkManager.Singleton.StartClient();
        RaceController.Instance.LocalPlayerIsReady();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        cameraUI.SetActive(false);
    }
}
