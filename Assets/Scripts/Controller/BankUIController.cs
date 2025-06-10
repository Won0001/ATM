using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BankUIController : MonoBehaviour
{
    UserDataManager userDataManager =  new UserDataManager();
    
    [Header("메인메뉴")]
    public GameObject mainMenuUI;
    public GameObject popupLoginUI;
    public Button depositButton;
    public Button withdrawButton;
    public Button remittanceMenu;
    public Button logOutButton;

    [Header("입금버튼메뉴")] 
    public GameObject depositMainMenu;
    public Button deposit10000;
    public Button deposit30000;
    public Button deposit50000;
    public TMP_InputField depositAmount;
    public Button depositInput;
    public Button depositBackButton;
    
    [Header("출금버튼메뉴")]
    public GameObject withdrawMainMenu;
    public Button withdraw10000;
    public Button withdraw30000;
    public Button withdraw50000;
    public TMP_InputField withdrawAmount;
    public Button withdrawInput;
    public Button withdrawBackButton;
    
    [Header("송금버튼메뉴")]
    public GameObject remittanceMainMenu;
    public TMP_InputField inputTargetID;
    public TMP_InputField inputRemittanceMoney;
    public Button remittanceButton;
    public Button remittanceBackButton;
    
    [Header("오류메세지")]
    public GameObject panel;
    
    private void Start()
    {
        depositMainMenu.SetActive(false);
        withdrawMainMenu.SetActive(false);
        remittanceMainMenu.SetActive(false);
        
        depositButton.onClick.AddListener(ShowDepositMainMenu);
        withdrawButton.onClick.AddListener(ShowWithdrawMainMenu);
        depositBackButton.onClick.AddListener(ShowMainMenu);
        withdrawBackButton.onClick.AddListener(ShowMainMenu);
        logOutButton.onClick.AddListener(ShowLoginPage);
        remittanceMenu.onClick.AddListener(ShowRemittanceMainMenu);

        deposit10000.onClick.AddListener(() => Deposit(10000));
        deposit30000.onClick.AddListener(() => Deposit(30000));
        deposit50000.onClick.AddListener(() => Deposit(50000));
        depositInput.onClick.AddListener(DepositInput);
        
        withdraw10000.onClick.AddListener(() => Withdraw(10000));
        withdraw30000.onClick.AddListener(() => Withdraw(30000));
        withdraw50000.onClick.AddListener(() => Withdraw(50000));
        withdrawInput.onClick.AddListener(WithdrawInput);

        remittanceButton.onClick.AddListener(Remittance);
        remittanceBackButton.onClick.AddListener(ShowMainMenu);
    }
    
    void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        depositMainMenu.SetActive(false);
        withdrawMainMenu.SetActive(false);
        remittanceMainMenu.SetActive(false);
    }

    void ShowDepositMainMenu()
    {
        mainMenuUI.SetActive(false);
        depositMainMenu.SetActive(true);
        withdrawMainMenu.SetActive(false);
        remittanceMainMenu.SetActive(false);
    }
    
    void ShowWithdrawMainMenu()
    {
        mainMenuUI.SetActive(false);
        depositMainMenu.SetActive(false);
        withdrawMainMenu.SetActive(true);
        remittanceMainMenu.SetActive(false);
    }

    void ShowRemittanceMainMenu()
    {
        mainMenuUI.SetActive(false);
        depositMainMenu.SetActive(false);
        withdrawMainMenu.SetActive(false);
        remittanceMainMenu.SetActive(true);
    }

    void ShowLoginPage()
    {
        gameObject.SetActive(false);
        popupLoginUI.SetActive(true);
    }

    void Deposit(int amount)
    {
        if (amount <= GameManager.Instance.userData.cash && amount > 0)
        {
            GameManager.Instance.userData.balance += amount;
            GameManager.Instance.userData.cash -= amount;
            
            GameManager.Instance.Refresh();
            GameManager.Instance.SaveCurrentUserData();
        }
    }

    void DepositInput()
    {
        string input = depositAmount.text;
        int amount;

        if (int.TryParse(input, out amount) && amount > 0)
        {
            if (amount <= GameManager.Instance.userData.cash)
            {
                GameManager.Instance.userData.balance += amount;
                GameManager.Instance.userData.cash -= amount;
                
                GameManager.Instance.Refresh();
                GameManager.Instance.SaveCurrentUserData();
            }
            else if (amount > GameManager.Instance.userData.cash)
            {
                StartCoroutine(ShowPanel("금액을 초과하였습니다"));
            }
        }
        else
        {
            StartCoroutine(ShowPanel("입력 정보를 확인해 주세요"));
        }
    }

    void Withdraw(int amount)
    {
        if (amount <= GameManager.Instance.userData.balance && amount > 0)
        {
            GameManager.Instance.userData.balance -= amount;
            GameManager.Instance.userData.cash += amount;
            
            GameManager.Instance.Refresh();
            GameManager.Instance.SaveCurrentUserData();
        }
    }

    void WithdrawInput()
    {
        string input = withdrawAmount.text;
        int amount;

        if (int.TryParse(input, out amount) && amount > 0)
        {
            if (amount <= GameManager.Instance.userData.balance)
            {
                GameManager.Instance.userData.balance -= amount;
                GameManager.Instance.userData.cash += amount;
                
                GameManager.Instance.Refresh();
                GameManager.Instance.SaveCurrentUserData();
            }
            else if (amount > GameManager.Instance.userData.balance)
            {
                StartCoroutine(ShowPanel("금액을 초과하였습니다"));
            }
        }
        else
        {
            StartCoroutine(ShowPanel("입력 정보를 확인해 주세요"));
        }
    }

    void Remittance()
    {
        var users = userDataManager.LoadAllUser();
        var curUser = GameManager.Instance.userData;
        string targetID = inputTargetID.text;

        if (!int.TryParse(inputRemittanceMoney.text, out int amount) || amount <= 0)
        {
            StartCoroutine(ShowPanel("입력 정보를 확인해 주세요"));
            return;
        }

        if (targetID == curUser.identification)
        {
            StartCoroutine(ShowPanel("본인에게 송금할 수 없습니다"));
            return;
        }
        
        UserData targetUser = users.Find(t => t.identification == targetID);

        if (targetUser == null)
        {
            StartCoroutine(ShowPanel("입력 정보를 확인해 주세요"));
            return;
        }

        if (amount > curUser.balance)
        {
            StartCoroutine(ShowPanel("금액이 부족합니다"));
            return;
        }

        if (targetUser != null && amount <= curUser.balance)
        {
            curUser.balance -= amount;
            targetUser.balance += amount;
        }

        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].identification == curUser.identification)
            {
                users[i] = curUser;
            }
            
            if (users[i].identification == targetUser.identification)
            {
                users[i] = targetUser;
            }
        }
        
        userDataManager.SaveAllUser(users);
        GameManager.Instance.Refresh();
    }

    IEnumerator ShowPanel(string message)
    {
        panel.SetActive(true);
        panel.GetComponentInChildren<TextMeshProUGUI>().text = message;
        yield return new WaitForSeconds(2f);
        panel.SetActive(false);
    }
}
