// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
using System.Collections.Generic;

public class Account
{
    private decimal _balance;
    private List<INotifyer> _notifyers;

    public Account()
    {
        _balance = 0;
        _notifyers = new List<INotifyer>();
    }

    public Account(decimal in_balance)
    {
        _balance = in_balance;
        _notifyers = new List<INotifyer>();
    }

    public void AddNotifyer(INotifyer notifyer)
    {
        _notifyers.Add(notifyer);
    }

    public void ChangeBalance(decimal value)
    {
        _balance = value;
        Notification();
    }

    public decimal Balance()
    {
        return _balance;
    }

    private void Notification()
    {
        foreach (var notifyer in _notifyers)
        {
            notifyer.Notify(_balance);
        }
    }
}

public interface INotifyer
{
    void Notify(decimal balance);
}

public class SMSLowBalanceNotifyer : INotifyer
{
    private string _phone;
    private decimal _lowBalanceValue;

    public SMSLowBalanceNotifyer(string phone, decimal lowBalanceValue)
    {
        _phone = phone;
        _lowBalanceValue = lowBalanceValue;
    }

    public void Notify(decimal balance)
    {
        if (balance < _lowBalanceValue)
        {
            Console.WriteLine($"SMSLowBalanceNotifyer: Balance is {balance}");
        }
    }
}

public class EMailBalanceChangedNotifyer : INotifyer
{
    private string _email;

    public EMailBalanceChangedNotifyer(string email)
    {
        _email = email;
    }

    public void Notify(decimal balance)
    {
        Console.WriteLine($"EMailBalanceChangedNotifyer: Balance is {balance}");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Account account = new Account(1000);

        SMSLowBalanceNotifyer smsNotifyer = new SMSLowBalanceNotifyer("+123456789", 500);
        EMailBalanceChangedNotifyer emailNotifyer = new EMailBalanceChangedNotifyer("example@example.com");

        account.AddNotifyer(smsNotifyer);
        account.AddNotifyer(emailNotifyer);

        account.ChangeBalance(500); // Should trigger both sms and email notifications
        account.ChangeBalance(200); // Should trigger only sms notification
        account.ChangeBalance(1000); // Should not trigger any notification
    }
}