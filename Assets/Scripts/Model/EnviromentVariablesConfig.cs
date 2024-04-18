using System.IO;
using UnityEngine;

public enum DevelopmentEnviroment{
    local,
    production
}

public class EnviromentVariablesConfig : MonoBehaviour
{
    [SerializeField] private DevelopmentEnviroment _currentEnviroment;

    // Start is called before the first frame update
    void Awake()
    {
        LoadConfig();
        

    }

    void Start()
    {
        ARLLQuestionModel aRLLQuestionModel = ARLLQuestionModel.GetARLLQuestionById(1);
        Debug.Log(aRLLQuestionModel.efficiency);
    }

    private void LoadConfig()
    {
        string filePath;

        // Check if the application is running on Android
        if (Application.platform == RuntimePlatform.Android)
        {
            // For Android, use a different configuration file path
            filePath = Path.Combine(Application.persistentDataPath, "config.json");
        }
        else
        {
            // For other platforms (e.g., Windows), use the original configuration file path
            if (_currentEnviroment == DevelopmentEnviroment.local)
            {
                filePath = Path.Combine(Application.dataPath, "config.local.json");
            }
            else
            {
                filePath = Path.Combine(Application.dataPath, "config.json");
            }
        }

        if (File.Exists(filePath))
        {
            // Read the external file
            string dataAsJson = File.ReadAllText(filePath);
            EnviromentVariablesManager loadedData = JsonUtility.FromJson<EnviromentVariablesManager>(dataAsJson);
            // Load variables to singleton
            EnviromentVariablesManager.Instance.Database = loadedData.Database;
            EnviromentVariablesManager.Instance.DBUsername = loadedData.DBUsername;
            EnviromentVariablesManager.Instance.DBPort = loadedData.DBPort;
            EnviromentVariablesManager.Instance.DBPassword = loadedData.DBPassword;
            EnviromentVariablesManager.Instance.DBHost = loadedData.DBHost;
        }
        else
        {
            Debug.LogError("Cannot find config file. Verify if 'config.json' or 'config.local.json' exists.");
        }
    }
}
