using Npgsql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryRawModel
{
    public int inventory_id { get; set; }
    public int user_id { get; set; }
    public int chassis_id { get; set; }
    public int wheel_id { get; set; }
    public int steeringWheel_id { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }

    public static List<InventoryRawModel> GetRawUserInventory(string userId)
    {

        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var userInventory = con.Query<InventoryRawModel>("select * from inventories where user_id = @userId;", new { userId }).ToList();
        con.Close();
        return userInventory;
    }

    // Get all the possible wheels that user doesn't have in his/her inventory.
    public static List<WheelModel> GetPossibleWheelsRewards(string userId)
    {

        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var possibleWheelRewards = con.Query<WheelModel>("select * from wheels w where wheel_id not in(select wheel_id from inventories where user_id = @userId AND wheel_id IS NOT NULL);", new { userId }).ToList();
        con.Close();
        return possibleWheelRewards;
    }

    public static List<ChassisModel> GetPossibleChassisRewards(string userId)
    {

        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var possibleChassisRewards = con.Query<ChassisModel>("select * from chassis c where chassis_id not in(select chassis_id from inventories where user_id = @userId AND chassis_id IS NOT NULL);", new { userId }).ToList();
        con.Close();
        return possibleChassisRewards;
    }

    public static List<SteeringWheelModel> GetPossibleSteeringWheelRewards(string userId)
    {
        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var possibleChassisRewards = con.Query<SteeringWheelModel>("select * from chassis c where chassis_id not in(select chassis_id from inventories where user_id = @userId AND chassis_id IS NOT NULL);", new { userId }).ToList();
        con.Close();
        return possibleChassisRewards;
    }

}


public class InventoryUser
{
    //public int inventory_id { get; set; }
    //public int user_id { get; set; }
    //public int? wheel_id { get; set; }
    //public string? wheel_name { get; set; }
    //public int? chassis_id { get; set; }
    //public string? chassis_name { get; set; }

    public int inventory_id { get; set; }
    public int user_id { get; set; }
    public int item_id { get; set; }
    public string item_name { get; set; }
    public string item_type { get; set; }

    public static List<InventoryUser> GetUserInventoryNerf(int userId)
    {
        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var userInventory = con.Query<InventoryUser>("select i.inventory_id,i.user_id,i.wheel_id,w.wheel_name,i.chassis_id, c.chassis_name from inventories i\r\nleft JOIN wheels w ON i.wheel_id = w.wheel_id left join chassis c ON i.chassis_id = c.chassis_id where user_id=@userId AND (i.wheel_id IS NOT NULL OR i.chassis_id IS NOT NULL);", new { userId }).ToList();
        con.Close();
        return userInventory;
    }

    public static List<InventoryUser> GetUserInventory(string userId)
    {
        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var userInventory = con.Query<InventoryUser>(@"
        SELECT 
            i.inventory_id, 
            i.user_id, 
            COALESCE(i.wheel_id, i.chassis_id) AS item_id, 
            COALESCE(w.wheel_name, c.chassis_name) AS item_name,
            CASE
                WHEN i.wheel_id IS NOT NULL AND i.chassis_id IS NULL THEN 'WHEEL'
                WHEN i.wheel_id IS NULL AND i.chassis_id IS NOT NULL THEN 'CHASSIS'
            END AS item_type
        FROM inventories i
        LEFT JOIN wheels w ON i.wheel_id = w.wheel_id
        LEFT JOIN chassis c ON i.chassis_id = c.chassis_id
        WHERE i.user_id = @userId AND (i.wheel_id IS NOT NULL OR i.chassis_id IS NOT NULL)
        ORDER BY i.inventory_id;
        ", new { userId }).ToList();
        con.Close();
        return userInventory;
    }

    public static void InsertWheelAwardToInventory(string userId,int wheelId)
    {
        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        con.Execute(@"insert into inventories
            values(default,@userId,null,@wheelId,null,default,default);", new { userId, wheelId });
        con.Close();
        
    }

    public static void InsertChassisAwardToInventory(string userId, int chassisId)
    {
        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        con.Execute(@"insert into inventories
            values(default,@userId,@chassisId,null,null,default,default);", new { userId, chassisId });
        con.Close();

    }
}