using System;
//using Unity.VisualScripting.Dependencies.Sqlite;
//using Dapper.Contrib.Extensions;

public class ChassisModel
{
    public int chassis_id { get; set; }
    public string chassis_name { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }

}

