using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

public class ARLLQuestionModel
{
    public int arll_question_id { get; set; }
    public float initial_temperature { get; set; }
    public float number_moles { get; set; }
    public float initial_volume { get; set; }
    public float final_volume { get; set; }
    public float pressure { get; set; }
    public int arll_wheel_id { get; set; }
    public float wheel_mass { get; set; }
    public float heat_capacity { get; set; }
    public float work { get; set; }
    public float delta_temp { get; set; }
    public float amount_heatQ { get; set; }
    public float efficiency { get; set; }
    public float change_internal_energy { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }

    public static List<ARLLQuestionModel> GetARLLQuestions()
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var wheels = con.Query<ARLLQuestionModel>("SELECT * FROM arll_questions;").ToList();
        con.Close();
        return wheels;
    }

    public static ARLLQuestionModel GetARLLQuestionById(int arll_question_id)
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var wheel = con.QuerySingle<ARLLQuestionModel>($"SELECT * FROM arll_questions WHERE arll_question_id ={arll_question_id};");
        con.Close();
        return wheel;
    }
}
