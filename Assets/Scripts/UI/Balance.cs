using System;
using TMPro;
using UnityEngine;

public class Balance : MonoBehaviour
{
    public TextMeshProUGUI userInfoText;

    public void RefreshBalance()
    {
        var userData = GameManager.Instance.userData;

        if (userData != null)
        {
            string name = userData.userName;
            int balance = userData.balance;
        
            userInfoText.text = string.Format("{0}\n\n balance      {1:N0}", name, balance);
        }
    }
}
