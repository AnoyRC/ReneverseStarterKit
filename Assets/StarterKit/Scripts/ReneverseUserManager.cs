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
    [Header("Parent of the Panel")]
    public GameObject Parent;

    private List<User> users = new();

    public static User SelectedUser;
    
    public static ReneverseUserManager Instance;

    [Header("Selected User")]
    [SerializeField]
    private User selectedUser;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //Search OnValueChanged with delay
        SearchInput.GetComponent<TMP_InputField>().onValueChanged.AddListener(delegate 
        {
            ResetSearch();
            StartCoroutine(SearchUser(0.5f)); 
        });

        //Open Dropdown
        SearchInput.GetComponent<TMP_InputField>().onSelect.AddListener(delegate
        {
            StartCoroutine(TogglePanel(true, 0));
        });

        //Close Dropdown with delay
        SearchInput.GetComponent<TMP_InputField>().onDeselect.AddListener(delegate
        {
            StartCoroutine(TogglePanel(false, 2));
        });
    }

    // Search User after delay
    IEnumerator SearchUser(float time)
    {
        string term = SearchInput.GetComponent<TMP_InputField>().text;
        yield return new WaitForSeconds(time);
        yield return Search(term);
    }

    // Search User with Search Term provided
    // Dependencies : Reneverse Manager
    public async Task Search(string term)
    {
        if (term.Length == 0) return;
        try
        {
            if (term != SearchInput.GetComponent<TMP_InputField>().text) return;

            UsersResponse.UsersData usersData = await ReneverseManager.ReneAPI.User().Search(term);

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

    //Instantiate Users in Canvas
    void LoadUsers()
    {
        foreach (var entry in users)
        {
            var asset = Instantiate(UserPrefab);
            asset.transform.SetParent(Panel.transform, false);
            asset.GetComponent<UserPrefabManager>().Initialize(entry);
        }
    }

    //Select User
    public void SelectUser(string name, string userId)
    {
        User thisUser = new(name, userId);
        SelectedUser = thisUser;
        Serialize();
        SearchInput.GetComponent<TMP_InputField>().text = name;
        StartCoroutine(TogglePanel(false, 0));
    }

    //Clear List and UI
    private void ResetSearch()
    {
        users.Clear();

        foreach (Transform child in Panel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    //Drop down controller
    IEnumerator TogglePanel(bool Switch, float time)
    {
        yield return new WaitForSeconds(time);
        Parent.SetActive(Switch);
    }

    #region Show Selected User in Inspector
    public void Serialize()
    {
        selectedUser.Name = SelectedUser.Name;
        selectedUser.UserId = SelectedUser.UserId;
    }
    #endregion
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
