using UnityEngine;
using UnityEngine.SceneManagement;

public class Interfaces : MonoBehaviour
{
    public void PlayScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
