using System;
using System.Collections.Generic;
using System.IO;

public class User
{
    // Private fields
    private int id;
    private string username;
    private string email;
    private string password;
    private string role;
    private string adminpassword = "Password1234";

    // Shared fields for user management
    protected static string filePath = "users.csv";
    protected static List<User> users = new List<User>();

    // Properties
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    public string Username
    {
        get { return username; }
        set { username = value; }
    }
    public string Email
    {
        get { return email; }
        set { email = value; }
    }
    public string Password
    {
        get { return password; }
        set { password = value; }
    }
    public string Role
    {
        get { return role; }
        set { role = value; }
    }
    public string Adminpassword
    {
        get { return adminpassword; }
    }

    // Full constructor (5 parameters)
    public User(int id, string username, string email, string password, string role)
    {
        this.id = id;
        this.username = username;
        this.email = email;
        this.password = password;
        this.role = role;
    }

    // Constructor for Admin inheritance (3 parameters)
    public User(string username, string password, string role)
    {
        this.id = 0;
        this.username = username;
        this.email = string.Empty;
        this.password = password;
        this.role = role;
    }

    // Simplified constructor (2 parameters) - needed by LoadUsersFromCsv
    public User(string username, string password)
    {
        this.id = 0;
        this.username = username;
        this.email = string.Empty;
        this.password = password;
        this.role = "Student";
    }

    public void Logout()
    {
        Console.WriteLine("User logged out.");
    }

    public void UpdateProfile()
    {
        Console.WriteLine("User profile updated.");
    }

    protected void LoadUsersFromCsv()
    {
        if (!File.Exists(filePath))
            return;

        users.Clear(); // Clear existing users before loading

        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines)
        {
            string[] data = line.Split(',');

            if (data.Length == 2)
            {
                users.Add(new User(data[0], data[1]));
            }
        }
    }

    protected void SaveUsersToCsv()
    {
        List<string> lines = new List<string>();

        foreach (User user in users)
        {
            lines.Add($"{user.Username},{user.Password}");
        }

        File.WriteAllLines(filePath, lines);
    }
}