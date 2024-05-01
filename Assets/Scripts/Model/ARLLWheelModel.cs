using Npgsql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

public class ARLLWheelModel
{
    public int arll_wheel_id { get; set; }
    public string arll_wheel_name { get; set; }
    public float specific_heat { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }

    public static List<ARLLWheelModel> GetARLLWheels()
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var arllWheels = con.Query<ARLLWheelModel>("SELECT * FROM arll_wheels;").ToList();
        con.Close();
        return arllWheels;
    }

    public static ARLLWheelModel GetARLLWheelById(int arll_wheel_id)
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var arllWheel = con.QuerySingle<ARLLWheelModel>($"SELECT * FROM arll_wheels WHERE arll_wheel_id = {arll_wheel_id};");
        con.Close();
        return arllWheel;
    }
}

