public static class LoggedUser
{

    private static string _username;
    private static string _userCode;

    public static string Username => _username;
    public static string UserCode => _userCode;

    public static void LogInUser(string userCode, string username)
    {
        _userCode = userCode;
        _username = username;
    }

}
