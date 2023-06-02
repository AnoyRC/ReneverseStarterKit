using Rene.Sdk.Api.User.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ReneverseUserManager : MonoBehaviour
{
    [Header("Parent where prefab will spawn")]
    public GameObject Panel;
    [Header("UI Prefab of the asset")]
    public GameObject UserPrefab;
    [Header("Search Term Input Field")]
    public GameObject SearchInput;

    public User SelectedUser;

    private List<User> users = new();

    // Start is called before the first frame update
    void Start()
    {
        SearchInput.GetComponent<TMP_InputField>().onValueChanged.AddListener(delegate 
        { 
            SearchUser(); 
        });
    }

    async void SearchUser()
    {
        string term = SearchInput.GetComponent<TMP_InputField>().text;
        await Search(term);
    }

    public async Task Search(string term)
    {
        users.Clear();
        try
        {
            UsersResponse.UsersData usersData = await ReneverseManager.ReneAPI.User().Search(term);
            Debug.Log(usersData.Items.Count);
            foreach (UserResponse.UserData user in usersData.Items)
            {
                User thisUser = new(user.Data.FirstName + " " + user.Data.LastName, user.UserId);
                users.Add(thisUser);
            }

            LoadUsers();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    void LoadUsers()
    {
        foreach (var entry in users)
        {
            var asset = Instantiate(UserPrefab);
            asset.transform.SetParent(Panel.transform, false);
            asset.GetComponent<UserPrefabManager>().Initialize(entry);
        }
    }
}

[Serializable]
public class User
{
    public string Name;
    public string UserId;
    
    public User(string name, string userId)
    {
        Name = name;
        UserId = userId;
    }
}
