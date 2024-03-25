using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
public class WheelModel
{
    public int wheel_id { get; set; }
    public string wheel_name { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }

    public static List<WheelModel> GetWheels()
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var wheels = con.Query<WheelModel>("SELECT * FROM wheels;").ToList();
        con.Close();
        return wheels;
    }

    public static WheelModel GetWheelById(int wheel_id)
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var wheel = con.QuerySingle<WheelModel>($"SELECT * FROM wheels WHERE wheel_id ={wheel_id};");
        con.Close();
        return wheel;
    }
}
