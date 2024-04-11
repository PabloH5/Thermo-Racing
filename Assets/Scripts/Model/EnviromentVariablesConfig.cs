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

    private void LoadConfig()
    {

        string filePath;

        // Read the selected configuration file.
        if(_currentEnviroment == DevelopmentEnviroment.local){
            filePath = Path.Combine(Application.dataPath, "config.local.json");
        }else{
            filePath = Path.Combine(Application.dataPath, "config.json");
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
            Debug.LogError("Cannot find config file. Verify if 'config.json' exists.");
        }
    }
}
