using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

// La clase User representa la estructura de la tabla "users" en tu base de datos.
public class UserModel
{
    public string user_id { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public int? current_wheels { get; set; }
    public int? current_chassis { get; set; }
    public int? current_steeringWheel { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }


    public static UserModel GetUserById(string userID)
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var user = con.QuerySingleOrDefault<UserModel>("SELECT * FROM users WHERE user_id = @userID;", new { userID });
        con.Close();
        return user;
    }

    public static List<UserModel> GetUsers()
    {
        using NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var users = con.Query<UserModel>("SELECT * FROM users;").ToList();
        con.Close();
        return users;
    }


    public static bool VerifyExistUsername(string username)
    {
        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var existsInBD = con.QuerySingleOrDefault<bool>("SELECT EXISTS(SELECT 1 FROM users WHERE username = @username);", new { username });
        con.Close();
        return existsInBD;
    }

    public static bool VerifyExistUserByCode(string userCode)
    {
        using NpgsqlConnection con = DBController.EstablishConnectionDB();

        con.Open();
        var existsInBD = con.QuerySingleOrDefault<bool>("SELECT EXISTS(SELECT 1 FROM users WHERE user_id = @userCode);", new { userCode });
        con.Close();
        return existsInBD;
    }

    public static UserModel CreateUser(string userID, string username, string password)
    {
        using NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var user = con.QuerySingle<UserModel>($"INSERT INTO users VALUES (@user_id,@username,@password,null,null,null,default,null) RETURNING *;", new { user_id = userID, username, password });
        con.Close();
        return user;
    }
}

