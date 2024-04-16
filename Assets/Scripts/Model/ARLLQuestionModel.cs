using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

public class ARLLQuestionModel
{
    public int arll_question_id { get; set; }
    public int arll_wheel_id { get; set; }
    public float heat_capacity { get; set; }
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
        var arllQuestion = con.QuerySingle<ARLLQuestionModel>($"SELECT aq.arll_question_id,aq.arll_wheel_id,aq.heat_capacity,aq.\"amount_heatQ\",aq.efficiency, aq.change_internal_energy,aw.arll_wheel_name,aw.specific_heat FROM arll_questions as aq INNER JOIN arll_wheels as aw ON aq.arll_wheel_id = aw.arll_wheel_id WHERE aq.arll_wheel_id =(SELECT aq.arll_wheel_id FROM arll_questions aq WHERE aq.arll_question_id=@QuestionId);",new { QuestionId = arll_question_id });
        con.Close();
        return arllQuestion;
    }
}
