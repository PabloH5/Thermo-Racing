using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInController : MonoBehaviour
{
    [Header("UI Log In form")]

    [Space(10)]
    [SerializeField] private TMP_InputField codeInput;
    [SerializeField] private TMP_Text userCodeValidationMessage;
    [SerializeField] private RawImage userCodeValidationMark;
    [SerializeField] private RawImage userCodeValidationError;

    [Space(10)]
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text passwordValidationMessage;
    [SerializeField] private RawImage passwordValidationMark;
    [SerializeField] private RawImage passwordValidationError;

    public void LogInUser()
    {

        string code = codeInput.text.Trim();
        string password = passwordInput.text.Trim();

        // * --------------- USER CODE VALIDATIONS --------------------
        // Validate the user code must have 7 digits.
        if (CountChars(code) != 7)
        {
            userCodeValidationMessage.text = "El código debe tener 7 dígitos.";
            userCodeValidationError.gameObject.SetActive(true);
            return;
        }

        userCodeValidationMessage.text = "";
        userCodeValidationError.gameObject.SetActive(false);
        userCodeValidationMark.gameObject.SetActive(true);

        //  * --------------- PASSWORD VALIDATIONS --------------------

        if (password == "")
        {
            passwordValidationMessage.text = "Este campo no puede estar vacío.";
            passwordValidationError.gameObject.SetActive(true);
            return;
        }

        passwordValidationMessage.text = "";
        passwordValidationError.gameObject.SetActive(false);
        passwordValidationMark.gameObject.SetActive(true);

        //* --------------- DATABASES VALIDATIONS ---------------
        // Validate the password is the same to DB.
        UserModel userDB = UserModel.GetUserById(code);

        if (userDB == null)
        {
            userCodeValidationMessage.text = "El código ingresado no está asociado a ninguna cuenta. Regístrelo";
            userCodeValidationError.gameObject.SetActive(true);
            return;
        }

        userCodeValidationMessage.text = "";
        userCodeValidationError.gameObject.SetActive(false);
        userCodeValidationMark.gameObject.SetActive(true);

        // Compare the hash saved in DB with the user password input.
        bool verifyPassword = BCrypt.Net.BCrypt.Verify(password,userDB.password);
        if(!verifyPassword)
        {
            passwordValidationMessage.text = "La contraseña no es correcta. Inténtalo nuevamente.";
            passwordValidationError.gameObject.SetActive(true);
            return;
        }

        passwordValidationMessage.text = "";
        passwordValidationError.gameObject.SetActive(false);
        passwordValidationMark.gameObject.SetActive(true);

        // Save the logged user data in memory.
        LoggedUser.LogInUser(userDB.user_id, userDB.username);
        // Change to Home menu
        SceneManager.LoadScene("MainMenu");
        //SceneManager.LoadScene("ThermoEcuacionador");

    }


    public int CountChars(string str)
    {
        int letterCount = str.Count(char.IsLetter);
        int digitCount = str.Count(char.IsDigit);
        int specialCount = str.Count(c => !char.IsLetterOrDigit(c));
        return letterCount + digitCount + specialCount;
    }


}
