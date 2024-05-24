using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interfaces : MonoBehaviour
{
    [SerializeField] private GameObject CanvasToOff;
    [SerializeField] private GameObject CanvasToOn;
    [SerializeField] private GameObject CanvasBack;
    [SerializeField] private string sceneToBack;
    [SerializeField] private TMP_Text usernameTextUI;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            usernameTextUI.text = $"Bienvenido: {LoggedUser.Username} - {LoggedUser.UserCode}";
        }
    }
    public void PlayScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void Acivate()
    {
        CanvasToOn.SetActive(true);
    }
    public void Desactivate()
    {
        CanvasToOff.SetActive(false);
    }

    public void ActivateAndDesactivate()
    {
        CanvasToOff.SetActive(false);
        CanvasToOn.SetActive(true);
    }
    public void ReverseActivateAndDesactivate()
    {
        CanvasBack.SetActive(true);
        CanvasToOff.SetActive(false);
    }

    public void GoBackButton()
    {
        SceneManager.LoadScene(sceneToBack);
    }

    
}
