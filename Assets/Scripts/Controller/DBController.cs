using Npgsql;
using UnityEngine;
using System;

public static class DBController
{

    private static string _dbConnectionString = "Host=localhost;Username=postgres;Password=admin123;Database=thermo-racing;Port=5432";

    public static NpgsqlConnection EstablishConnectionDB()
    {
        try
        {
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
