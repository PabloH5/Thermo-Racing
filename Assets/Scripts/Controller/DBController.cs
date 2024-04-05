using Npgsql;
using UnityEngine;
using System;

enum DBHostEnum {
    local,
    remote,
}

public static class DBController
{
    [SerializeField] private static DBHostEnum _dbHost;

    private static string _dbConnectionString;

    public static NpgsqlConnection EstablishConnectionDB()
    {
        try
        {
            //_dbHost = DBHostEnum.remote;

            if ( _dbHost == DBHostEnum.local)
            {
                _dbConnectionString = "Host=localhost;Username=postgres;Password=admin123;Database=thermo-racing;Port=5432";
            }
            else
            {
                _dbConnectionString = "Host=ep-mute-wave-a4a7obkm.us-east-1.aws.neon.tech;Username=thermo-racing_owner;Password=****;Database=thermo-racing;Port=5432";
            }

            Debug.Log(_dbConnectionString);

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
