using System;

// La clase User representa la estructura de la tabla "users" en tu base de datos.
public class UserModel
{
    public int user_id { get; set; }
    public string username { get; set; }
    public int? current_wheels { get; set; }
    public int? current_chassis { get; set; }
    public int? current_steeringWheel { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}

