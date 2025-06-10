using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class UserDataListWrapper
{
    public List<UserData> users = new List<UserData>();
}

public class UserDataManager
{
    public List<UserData> LoadAllUser()
    {
        string path = Application.persistentDataPath + "/UserData.json";
        if (!File.Exists(path)) return new List<UserData>();

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<UserDataListWrapper>(json).users;
    }

    public void SaveAllUser(List<UserData> users)
    {
        UserDataListWrapper wrapper = new UserDataListWrapper { users = users };
        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(Application.persistentDataPath + "/UserData.json", json);
    }
}
