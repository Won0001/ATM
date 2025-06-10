using System;
using TMPro;
using UnityEngine;

public class Cash : MonoBehaviour
{
    public TextMeshProUGUI cashText;

    public void RefreshCash()
    {
        var userData = GameManager.Instance.userData;
        if (userData != null)
        {
            int cash = userData.cash;
        
            cashText.text = string.Format("현금\n\n{0:N0}", cash);
        }
    }
}
