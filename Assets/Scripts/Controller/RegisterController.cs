using BCrypt.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class RegisterController : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_Text usernameValidationMessage;
    [Space(10)]
    [SerializeField] private TMP_InputField codeInput;
    [SerializeField] private TMP_Text userCodeValidationMessage;
    [Space(10)]
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text passwordValidationMessage;
    [Space(10)]
    [SerializeField] private TMP_InputField confirmpasswordInput;
    [SerializeField] private TMP_Text confirmPasswordValidationMessage;

    public void RegisterUser()
    {
        // Obtain the values from TextMeshPro textFields
        string username = usernameInput.text.Trim();
        string code = codeInput.text.Trim();
        string password = passwordInput.text.Trim();
        string confirmPassword = confirmpasswordInput.text.Trim();

        Debug.Log(username);
        // TODO: validate the username lenght
        if (username.Length >= 3 && username.Length <= 16)
        {
            Debug.Log("El texto es válido.");
        }

        // TODO: validate minimun and max username lenght
        //Regex regex = new Regex(@"^[0 - 9]{ 7 }$");
        //Debug.Log(regex.IsMatch(code));

        //// Validate the student code must have 7 characters and must have digits.
        //if (!regex.IsMatch(code))

        //{
        //    Debug.Log("The code must have 7 digits.");
        //    return;
        //}

        // Validate that password and confirmPassword are the same.
        if (password != confirmPassword)
        {
            Debug.Log("The passwords don't match.");
            return;
        }
        // TODO: Validate nickname in accordance with word list.


        // TODO: verify if nickname exists in DB.
        if (UserModel.VerifyExistUsername(username))
        {
            Debug.Log("The username already exists.");
            return;
        }



        // Hash the password.
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        // Save user in BD.
        //UserModel user = UserModel.CreateUser(code, username, hashedPassword);
        //Debug.Log(user.created_at);
    }
}
