using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserPrefabManager : MonoBehaviour
{
    [Header("User Details")]
    public User ThisUser;

    [Header("UI Inputs")]
    public TextMeshProUGUI Name;

    public ReneverseUserManager ReneverseUserManager;

    void Start()
    {
        ReneverseUserManager = ReneverseUserManager.Instance;
    }

    //Initialize the UI content
    public void Initialize(User user)
    {
        #region Setting User Details
        ThisUser = user;
        #endregion

        Name.text = ThisUser.Name;
    }

    public void SelectUser()
    {
        ReneverseUserManager.SelectUser(ThisUser.Name, ThisUser.UserId);
    }
}
