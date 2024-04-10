using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

public class TEQuestionModel
{
    public int te_question_id { get; set; }
    public string wording { get; set; }
    public string first_missing_word { get; set; }

#nullable enable
    public string? second_missing_word { get; set; }
    public string? third_missing_word { get; set; }
#nullable disable
    public string first_option { get; set; }
    public string second_option { get; set; }
    public string third_option { get; set; }
    public string fourth_option { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }

    public static List<TEQuestionModel> GetTEQuestions()
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var teQuestions = con.Query<TEQuestionModel>("SELECT * FROM te_questions;").ToList();
        con.Close();
        return teQuestions;
    }

    public static TEQuestionModel GetWheelById(int teQuestion_id)
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var teQuestion = con.QuerySingle<TEQuestionModel>($"SELECT * FROM te_questions WHERE te_question_id ={teQuestion_id};");
        con.Close();
        return teQuestion;
    }
}
