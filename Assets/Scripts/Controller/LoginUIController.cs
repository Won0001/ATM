using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIController : MonoBehaviour
{
    public GameObject popupBankUI;
    public GameObject signUpUI;
    public GameObject panel;
    
    public Button btnLogin;
    public Button btnSingUp;
    public TMP_InputField identification;
    public TMP_InputField password;
    
    UserDataManager userDataManager = new UserDataManager();

    private void Start()
    {
        btnLogin.onClick.AddListener(Login);
        btnSingUp.onClick.AddListener(SignUp);
    }

    public void Login()
    {
        var users = userDataManager.LoadAllUser();
        
        string inputId = identification.text;
        string inputPwd = password.text;

        if (string.IsNullOrEmpty(inputId) || string.IsNullOrEmpty(inputPwd))
        {
            StartCoroutine(ShowPanel("입력 정보를 확인해 주세요"));
            return;
        }
        
        UserData foundUser = users.Find(user => user.identification == inputId && user.password == inputPwd);

        if (foundUser == null)
        {
            StartCoroutine(ShowPanel("입력 정보를 확인해 주세요"));
            return;
        }
        
        GameManager.Instance.userData = foundUser;
        GameManager.Instance.Refresh();
        gameObject.SetActive(false);
        popupBankUI.SetActive(true);
    }

    public void SignUp()
    {
        gameObject.SetActive(false);
        signUpUI.SetActive(true);
    }
    
    IEnumerator ShowPanel(string message)
    {
        panel.SetActive(true);
        panel.GetComponentInChildren<TextMeshProUGUI>().text = message;
        yield return new WaitForSeconds(2f);
        panel.SetActive(false);
    }
}
