using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Unity.VisualScripting.Dependencies.Sqlite;
//using Dapper.Contrib.Extensions;

public class ChassisModel
{
    public int chassis_id { get; set; }
    //[Column("chassis_name")]
    public string chassis_name { get; set; }
    //[Column("created_at")]
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }

}

