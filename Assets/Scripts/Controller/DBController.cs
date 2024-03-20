using Npgsql;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dapper;

public class DBController : MonoBehaviour
{

    private static string _dbConnectionString = "Host=localhost;Username=postgres;Password=admin123;Database=thermo-racing;Port=5432";

    // Start is called before the first frame update
    void Start()
    {
        //List<string> ayuda = DBConnection.EstablishConnectionDB();
        //foreach (var item in ayuda)
        //{

        //    Debug.Log(item[1]);
        //}
        EstablishConnectionDB();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void EstablishConnectionDB()
    {
        var con = new NpgsqlConnection(_dbConnectionString);

        con.Open();

        var chassis = con.Query<ChassisModel>("SELECT * FROM chassis;");
        var steering_wheels = con.Query<SteeringWheelModel>("SELECT * FROM steering_wheels;");
        var users = con.Query<UserModel>("SELECT * FROM users;");

        foreach (var item in chassis)
        {
            Debug.Log($"ID: {item.chassis_id} - name:{item.chassis_name} - CA: {item.created_at}");
        }

        foreach (var item in steering_wheels)
        {
            Debug.Log($"ID: {item.steeringWheel_id} - name:{item.steeringWheel_name} - CA: {item.created_at}");
        }

        foreach (var item in users)
        {
            Debug.Log($"ID: {item.user_id} - name:{item.username}");
        }
        //Debug.Log(test);
    }
}
