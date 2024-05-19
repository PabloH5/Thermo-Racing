using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public enum DevelopmentEnviroment
{
    local,
    production
}

public class EnviromentVariablesConfig : MonoBehaviour
{
    [SerializeField] private DevelopmentEnviroment _currentEnviroment;
    // Text in Canvas to debug information in Android
    //[SerializeField] private TextMeshProUGUI TextAndroid;

    // Start is called before the first frame update
    void Awake()
    {
        LoadConfig();
    }

    // This method allow to read files in StreamingAssets folder in Android
    IEnumerator LoadConfigFromStreamingAssets()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "config.json");

        // Using UnityWebRequest, we can load files in StreamingAssets folder.
        using (UnityWebRequest request = UnityWebRequest.Get(filePath))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError("Error al cargar el archivo: " + request.error);
            }
            else
            {
                // Acciones con el contenido del archivo
                string data = request.downloadHandler.text;
                Debug.Log("Contenido del Config: " + data);
                EnviromentVariablesManager loadedData = JsonUtility.FromJson<EnviromentVariablesManager>(data);
                // Load variables to singleton
                EnviromentVariablesManager.Instance.Database = loadedData.Database;
                EnviromentVariablesManager.Instance.DBUsername = loadedData.DBUsername;
                EnviromentVariablesManager.Instance.DBPort = loadedData.DBPort;
                EnviromentVariablesManager.Instance.DBPassword = loadedData.DBPassword;
                EnviromentVariablesManager.Instance.DBHost = loadedData.DBHost;
            }
        }
    }

    private void LoadConfig()
    {
        string filePath;
        // Check if the application is running on Android
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("I'm Android");
            // Load sensible data from /StreamingAssets/config.json
            StartCoroutine(LoadConfigFromStreamingAssets());
            
        }
        else
        {
            Debug.Log("I'm not Android");
            // For other platforms (e.g., Windows), use the original configuration file path

            if (_currentEnviroment == DevelopmentEnviroment.local)
            {
                filePath = Path.Combine(Application.dataPath, "config.local.json");
                Debug.Log(filePath);
            }
            else
            {
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
                Debug.LogError("Cannot find config file. Verify if 'config.json' or 'config.local.json' exists.");
            }
        }


    }
}
