// Author Samuel McFadden

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
        Console.WriteLine("=== Logout ===\n");
        Console.WriteLine("Logging out...");
        Environment.Exit(0);

    }

    public void UpdateProfile()
    {
        Console.Clear();
        Console.WriteLine("=== Update Profile ===\n");

        // Find the current user in the users list
        User currentUser = users.Find(u => u.Username == this.username);

        if (currentUser == null)
        {
            Console.WriteLine("User not found in database.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        // Display current information
        Console.WriteLine("Current Profile Information:");
        Console.WriteLine($"Username: {currentUser.Username}");
        Console.WriteLine($"Password: {currentUser.Password}");
        Console.WriteLine();

        // Get new username
        Console.Write("Enter new username (or press Enter to keep current): ");
        string newUsername = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newUsername))
        {
            // Check if new username already exists
            if (users.Exists(u => u.Username == newUsername && u.Username != currentUser.Username))
            {
                Console.WriteLine("Username already exists. Update cancelled.");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }
            currentUser.Username = newUsername;
            this.username = newUsername;
        }

        // Get new password
        Console.Write("Enter new password (or press Enter to keep current): ");
        string newPassword = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            currentUser.Password = newPassword;
            this.password = newPassword;
        }

        // Save changes to CSV
        SaveUsersToCsv();

        Console.WriteLine("\nProfile updated successfully!");
        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
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

