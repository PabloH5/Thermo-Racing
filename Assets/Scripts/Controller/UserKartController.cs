using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserKartController : MonoBehaviour
{

    [SerializeField] private CustomManager customManager;
    // Start is called before the first frame update
    void Start()
    {
        // GET THE INFORMATION FROM CURRENT USER
        LoggedUser.LogInUser("2222222", "test");
        UserModel user = UserModel.GetUserById(LoggedUser.UserCode);
        Debug.Log(user.current_wheels);
        Debug.Log(user.current_chassis);
        customManager.ModifyEnumCustom(user.current_wheels, user.current_chassis);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
