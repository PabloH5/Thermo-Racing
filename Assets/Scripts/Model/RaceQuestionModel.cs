using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

public class RaceQuestionModel
{

    public int race_question_id { get; set; }
    public string wording { get; set; }
    public string first_option { get; set; }
    public string second_option { get; set; }
    public string third_option { get; set; }
    public string fourth_option { get; set; }
    public string correct_option { get; set; }
    public DateTime created_at { get; set; }
    public DateTime? updated_at { get; set; }

    public static List<RaceQuestionModel> GetAllRaceQuestions()
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var raceQuestions = con.Query<RaceQuestionModel>("SELECT * FROM race_questions;").ToList();
        con.Close();
        return raceQuestions;
    }

    public static RaceQuestionModel GetRaceQuestionById(int teQuestion_id)
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var raceQuestion = con.QuerySingle<RaceQuestionModel>($"SELECT * FROM race_questions WHERE te_question_id ={teQuestion_id};");
        con.Close();
        return raceQuestion;
    }

    public static List<RaceQuestionModel> GetRaceQuestions()
    {
        NpgsqlConnection con = DBController.EstablishConnectionDB();
        con.Open();
        var raceQuestions = con.Query<RaceQuestionModel>($"SELECT * FROM race_questions ORDER BY RANDOM() LIMIT 3;").ToList();
        con.Close();
        return raceQuestions;
    }
}
