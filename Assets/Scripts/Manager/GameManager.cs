using System;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public UserData userData;
    public Cash refreshCash;
    public Balance refreshBalance;
    public GameObject bankUI;
    
    UserDataManager userDataManager = new UserDataManager();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public void Refresh()
    {
        refreshCash.RefreshCash();
        refreshBalance.RefreshBalance();
    }

    public void SaveCurrentUserData()
    {
        var users = userDataManager.LoadAllUser();

        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].identification == userData.identification)
            {
                users[i] = userData;
                break;
            }
        }
        
        userDataManager.SaveAllUser(users);
    }
}
