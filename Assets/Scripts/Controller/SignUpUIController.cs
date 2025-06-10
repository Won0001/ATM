using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpUIController : MonoBehaviour
{
    public GameObject popupLoginUI;
    public GameObject panel;
    
    public TMP_InputField identification;
    public TMP_InputField password;
    public TMP_InputField userName;
    public TMP_InputField balance;
    public TMP_InputField cash;

    public Button submitButton;
    
    UserDataManager userDataManager = new UserDataManager();

    private void Start()
    {
        submitButton.onClick.AddListener(SignUp);
    }

    void SignUp()
    {
        if (string.IsNullOrWhiteSpace(identification.text) ||
            string.IsNullOrWhiteSpace(password.text) ||
            string.IsNullOrWhiteSpace(userName.text) ||
            string.IsNullOrWhiteSpace(balance.text) ||
            string.IsNullOrWhiteSpace(cash.text))
        {
            StartCoroutine(ShowPanel("입력 정보를 확인해 주세요"));
            return;
        }
        
        if (IsIdDuplicate(identification.text))
        {
            StartCoroutine(ShowPanel("이미 존재하는 아이디입니다"));
            return;
        }

        if (!int.TryParse(balance.text, out int intBalance) || !int.TryParse(cash.text, out int intCash))
        {
            StartCoroutine(ShowPanel("숫자만 입력해 주세요"));
            return;
        }

        UserData newUser = new UserData(
            identification.text,
            password.text,
            userName.text,
            intBalance,
            intCash
        );
        
        var users = userDataManager.LoadAllUser();
        users.Add(newUser);
        userDataManager.SaveAllUser(users);
                
        gameObject.SetActive(false);
        popupLoginUI.SetActive(true);
    }
    
    IEnumerator ShowPanel(string message)
    {
        panel.SetActive(true);
        panel.GetComponentInChildren<TextMeshProUGUI>().text = message;
        yield return new WaitForSeconds(2f);
        panel.SetActive(false);
    }

    bool IsIdDuplicate(string id)
    {
        var users = userDataManager.LoadAllUser();
        return users.Exists(user => user.identification == id);
    }
}
