[System.Serializable]
public class UserData
{
    public string identification;
    public string password;
    public string userName;
    public int balance;
    public int cash;

    public UserData(string identification, string password, string userName, int balance, int cash)
    {
        this.identification = identification;
        this.password = password;
        this.userName = userName;
        this.balance = balance;
        this.cash = cash;
    }
}
