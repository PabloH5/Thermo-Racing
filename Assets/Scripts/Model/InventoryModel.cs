using Npgsql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryModel
{
    public int inventory_id { get; set; }
    public int user_id { get; set; }
    public int chassis_id { get; set; }
    public int wheel_id { get; set; }
    public int steeringWheel_id { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }

    public static List<InventoryModel> GetUserInventory(int userId)
    {
        
        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var userInventory = con.Query<InventoryModel>("select * from inventories where user_id = @userId;", new { userId }).ToList();
        con.Close();
        return userInventory;
    }

    // Get all the possible wheels that user doesn't have in his/her inventory.
    public static List<WheelModel> GetPossibleWheelsRewards(int userId)
    {

        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var possibleWheelRewards = con.Query<WheelModel>("select * from wheels w where wheel_id not in(select wheel_id from inventories where user_id = @userId AND wheel_id IS NOT NULL);", new { userId }).ToList();
        con.Close();
        return possibleWheelRewards;
    }

    public static List<ChassisModel> GetPossibleChassisRewards(int userId)
    {

        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var possibleChassisRewards = con.Query<ChassisModel>("select * from chassis c where chassis_id not in(select chassis_id from inventories where user_id = @userId AND chassis_id IS NOT NULL);", new { userId }).ToList();
        con.Close();
        return possibleChassisRewards;
    }

    public static List<SteeringWheelModel> GetPossibleSteeringWheelRewards(int userId)
    {

        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var possibleChassisRewards = con.Query<SteeringWheelModel>("select * from chassis c where chassis_id not in(select chassis_id from inventories where user_id = @userId AND chassis_id IS NOT NULL);", new { userId }).ToList();
        con.Close();
        return possibleChassisRewards;
    }

}
