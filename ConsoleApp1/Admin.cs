using System;
using System.Collections.Generic;
using System.IO;

public class Admin : User
{
    private DateTime loginDate;

    public DateTime LoginDate
    {
        get { return loginDate; }
        set { loginDate = value; }
    }

    public List<User> Users
    {
        get { return users; }
        set { users = value; }
    }

    public Admin(string username, string password, DateTime loginDate, string role)
        : base(username, password, role)
    {
        this.loginDate = loginDate;
        LoadUsersFromCsv();
    }

    public void AddUser()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        users.Add(new User(username, password));
        SaveUsersToCsv();

        Console.WriteLine("User added and saved to database.");
    }

    public void RemoveUser()
    {
        Console.Write("Enter username to remove: ");
        string username = Console.ReadLine();

        User user = users.Find(u => u.Username == username);

        if (user != null)
        {
            users.Remove(user);
            SaveUsersToCsv();
            Console.WriteLine("User removed from database.");
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }

    public void UpdateUser()
    {
        Console.Write("Enter username to update: ");
        string username = Console.ReadLine();

        User user = users.Find(u => u.Username == username);

        if (user != null)
        {
            Console.Write("Enter new password: ");
            user.Password = Console.ReadLine();

            Console.Write("Enter new username: ");
            user.Username = Console.ReadLine();

            Console.Write("Enter new email: ");
            user.Email = Console.ReadLine();

            Console.Write("Enter new role: ");
            user.Role = Console.ReadLine();

            SaveUsersToCsv();
            Console.WriteLine("User updated in database.");
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }
}