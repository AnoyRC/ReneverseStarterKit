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

    //Initialize the UI content
    public void Initialize(User user)
    {
        #region Setting User Details
        ThisUser = user;
        #endregion

        Name.text = ThisUser.Name;
    }
}
