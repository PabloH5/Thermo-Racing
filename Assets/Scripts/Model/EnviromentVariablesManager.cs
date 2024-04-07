using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentVariablesManager
{

    public string DBHost;
    public string DBUsername;
    public string DBPort;
    public string Database;
    public string DBPassword;

    private readonly static EnviromentVariablesManager _instance = new EnviromentVariablesManager();

    private EnviromentVariablesManager()
    {
    }

    public static EnviromentVariablesManager Instance
    {
        get
        {
            return _instance;
        }
    }
}
