using Npgsql;
using UnityEngine;
using System;

public static class DBController
{
    private static string _dbConnectionString;

    public static NpgsqlConnection EstablishConnectionDB()
    {
        try
        {
            // Read the values from singleton.
            _dbConnectionString = $"Host={EnviromentVariablesManager.Instance.DBHost};Username={EnviromentVariablesManager.Instance.DBUsername};Password={EnviromentVariablesManager.Instance.DBPassword};Database={EnviromentVariablesManager.Instance.Database};Port={EnviromentVariablesManager.Instance.DBPort}";

            var con = new NpgsqlConnection(_dbConnectionString);
            return con;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }

    }
}
