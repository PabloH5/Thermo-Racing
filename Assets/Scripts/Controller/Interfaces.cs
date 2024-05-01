using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interfaces : MonoBehaviour
{
    [SerializeField] private GameObject CanvasToOff;
    [SerializeField] private GameObject CanvasToOn;
    [SerializeField] private GameObject CanvasBack;
    public void PlayScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void Exit()
    {
        Application.Quit();
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
}
