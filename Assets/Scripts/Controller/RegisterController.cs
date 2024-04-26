using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterController : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_Text usernameValidationMessage;
    [SerializeField] private RawImage usernameValidationMark;
    [SerializeField] private RawImage usernameValidationError;

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
    [Space(10)]
    [SerializeField] private TMP_InputField confirmpasswordInput;
    [SerializeField] private TMP_Text confirmPasswordValidationMessage;
    [SerializeField] private RawImage confirmPasswordValidationMark;
    [SerializeField] private RawImage confirmPasswordValidationError;

    [Header("Vars")]
    [SerializeField] private static HashSet<string> bannedWords;

    public void Awake()
    {
        LoadBannedWords();
    }

    public void RegisterUser()
    {
        // Obtain the values from TextMeshPro textFields
        string username = usernameInput.text.Trim();
        string code = codeInput.text.Trim();
        string password = passwordInput.text.Trim();
        string confirmPassword = confirmpasswordInput.text.Trim();

        // * --------------- USERNAME VALIDATIONS --------------------
        // 1.1 validate the username lenght
        if (CountChars(username) < 3 || CountChars(username) > 16)
        {
            usernameValidationMessage.text = "El nombre de usuario debe tener entre 3 y 16 caracteres.";
            usernameValidationError.gameObject.SetActive(true);
            return;
        }

        // 1.2 Valid banned words in username
        if (ValidateBannedWordsUsername(username))
        {
            usernameValidationMessage.text = "El nombre NO puede contener palabras obscenas.";
            usernameValidationError.gameObject.SetActive(true);
            return;
        }
        // Clear the validation error messages.
        usernameValidationMessage.text = "";
        usernameValidationError.gameObject.SetActive(false);
        usernameValidationMark.gameObject.SetActive(true);


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

        // * --------------- PASSWORD VALIDATIONS --------------------

        if (password == "")
        {
            Debug.Log(" PASSWORD");
            passwordValidationMessage.text = "Este campo no puede estar vacío.";
            passwordValidationError.gameObject.SetActive(true);
            return;
        }

        passwordValidationMessage.text = "";
        passwordValidationError.gameObject.SetActive(false);
        passwordValidationMark.gameObject.SetActive(true);

        if (confirmPassword == "")
        {
            Debug.Log("CONFIRM PASSWORD");
            confirmPasswordValidationMessage.text = "Este campo no puede estar vacío.";
            confirmPasswordValidationError.gameObject.SetActive(true);
            return;
        }
        Debug.Log("CONFIRM PASSWORD VALIDATION PASADA");
        confirmPasswordValidationMessage.text = "";
        confirmPasswordValidationError.gameObject.SetActive(false);
        confirmPasswordValidationMark.gameObject.SetActive(true);

        // 3.1 Validate that password and confirmPassword are the same.
        if (password != confirmPassword)
        {
            passwordValidationMark.gameObject.SetActive(false);
            confirmPasswordValidationMark.gameObject.SetActive(false);
            passwordValidationMessage.text = "Las contraseñas no coinciden. Vuelve a intentarlo.";
            passwordValidationError.gameObject.SetActive(true);
            confirmPasswordValidationMessage.text = "Las contraseñas no coinciden. Vuelve a intentarlo.";
            confirmPasswordValidationError.gameObject.SetActive(true);
            return;
        }

        Debug.Log("CONTRASEÑA IGUAL");

        passwordValidationMessage.text = "";
        confirmPasswordValidationMessage.text = "";
        passwordValidationError.gameObject.SetActive(false);
        confirmPasswordValidationError.gameObject.SetActive(false);
        passwordValidationMark.gameObject.SetActive(true);
        confirmPasswordValidationMark.gameObject.SetActive(true);

        // Verify if nickname exists in DB.
        if (UserModel.VerifyExistUsername(username))
        {
            usernameValidationMessage.text = "El nombre de usuario ya existe. Elige otro.";
            usernameValidationError.gameObject.SetActive(true);
            return;
        }

        Debug.Log("Nombre único");

        usernameValidationError.gameObject.SetActive(false);
        usernameValidationMark.gameObject.SetActive(true);



        // Hash the password.
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        // Save user in BD.
        UserModel user = UserModel.CreateUser(code, username, hashedPassword);
        Debug.Log(user.created_at);

    }

    public int CountChars(string str)
    {
        int letterCount = str.Count(char.IsLetter);
        int digitCount = str.Count(char.IsDigit);
        int specialCount = str.Count(c => !char.IsLetterOrDigit(c));
        return letterCount + digitCount + specialCount;
    }

    public static void LoadBannedWords()
    {
        try
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "banned_words.txt");
            bannedWords = new HashSet<string>(File.ReadAllLines(filePath).Select(line => line.Trim().ToLower()));
        }
        catch (IOException ex)
        {
            Debug.Log("Error al leer el archivo: " + ex.Message);
            bannedWords = new HashSet<string>();  // Inicializa con un conjunto vacío si hay un error
        }
    }

    public bool ValidateBannedWordsUsername(string username)
    {
        bool test = bannedWords.Any(word => username.ToLower().Contains(word));
        Debug.Log($"Banned word detected: {test}");
        return test;
    }
}
