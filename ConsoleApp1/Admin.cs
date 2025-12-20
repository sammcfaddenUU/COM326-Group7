// Author Samuel McFadden

using System;
using System.Collections.Generic;
using System.IO;

public class Admin : User
{
    private DateTime loginDate;
    private const string questionFilePath = "Question.csv";
    private const string ADMIN_PASSWORD = "ADMIN1234";
    private bool isLoggedIn = false;

    public DateTime LoginDate
    {
        get { return loginDate; }
        set { loginDate = value; }
    }

    public bool IsLoggedIn
    {
        get { return isLoggedIn; }
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

    public bool Login()
    {
        Console.Clear();
        Console.WriteLine("=== Admin Login ===\n");
        Console.Write("Enter admin password: ");
        string enteredPassword = Console.ReadLine();

        if (enteredPassword == ADMIN_PASSWORD)
        {
            isLoggedIn = true;
            loginDate = DateTime.Now;
            Console.WriteLine("\nLogin successful!");
            Console.WriteLine($"Login Date: {loginDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return true;
        }
        else
        {
            Console.WriteLine("\nInvalid password. Login failed.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return false;
        }
    }

    public void AddUser()
    {
        if (!isLoggedIn)
        {
            Console.WriteLine("You must be logged in to perform this action.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

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
        if (!isLoggedIn)
        {
            Console.WriteLine("You must be logged in to perform this action.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

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
        if (!isLoggedIn)
        {
            Console.WriteLine("You must be logged in to perform this action.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

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

    public void AddquizCatagory()
    {
        if (!isLoggedIn)
        {
            Console.WriteLine("You must be logged in to perform this action.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter quiz category name: ");
        string categoryName = Console.ReadLine();
        Console.WriteLine($"Quiz category '{categoryName}' added.");

        using (var writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"{categoryName}");
        }
    }

    public void RemoveQuizCategory()
    {
        if (!isLoggedIn)
        {
            Console.WriteLine("You must be logged in to perform this action.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter quiz category name to remove: ");
        string deleteName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(deleteName))
        {
            Console.WriteLine("Category name cannot be empty.");
            return;
        }

        if (!File.Exists(filePath))
        {
            Console.WriteLine("No categories found.");
            return;
        }

        // Read all lines from the file
        var lines = File.ReadAllLines(filePath);
        var updatedLines = new List<string>();
        bool found = false;

        // Keep all lines except the one to remove
        foreach (var line in lines)
        {
            if (line.Trim().Equals(deleteName, StringComparison.OrdinalIgnoreCase) && !found)
            {
                found = true; // Remove only the first match
                continue;
            }
            updatedLines.Add(line);
        }

        if (found)
        {
            // Write the updated list back to the file
            File.WriteAllLines(filePath, updatedLines);
            Console.WriteLine($"Quiz category '{deleteName}' removed.");
        }
        else
        {
            Console.WriteLine($"Quiz category '{deleteName}' not found.");
        }
    }

    public void AddQuestion()
    {
        if (!isLoggedIn)
        {
            Console.WriteLine("You must be logged in to perform this action.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Question ID: ");
        if (!int.TryParse(Console.ReadLine(), out int questionId))
        {
            Console.WriteLine("Invalid Question ID. Please enter a number.");
            return;
        }

        Console.Write("Enter question text: ");
        string questionText = Console.ReadLine();

        Console.Write("Enter question options (pipe-separated, e.g., Option1|Option2|Option3): ");
        string questionOptions = Console.ReadLine();

        Console.Write("Enter correct answer: ");
        string correctAnswer = Console.ReadLine();

        Console.Write("Enter difficulty level: ");
        string difficulty = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(questionText) || string.IsNullOrWhiteSpace(correctAnswer))
        {
            Console.WriteLine("Question text and correct answer cannot be empty.");
            return;
        }

        // Check if file exists, if not create it with header
        if (!File.Exists(questionFilePath))
        {
            using (var writer = new StreamWriter(questionFilePath, false))
            {
                writer.WriteLine("QuestionID,QuestionText,QuestionOptions,CorrectAnswer,QuestionDifficulty");
            }
        }

        // Append the new question to the CSV file with proper formatting
        using (var writer = new StreamWriter(questionFilePath, true))
        {
            writer.WriteLine(FormatCsvLine(questionId.ToString(), questionText, questionOptions, correctAnswer, difficulty));
        }

        Console.WriteLine($"Question with ID '{questionId}' added successfully.");
    }

    // Helper method to format CSV line with proper quoting
    private static string FormatCsvLine(params string[] fields)
    {
        var formattedFields = new List<string>();
        
        foreach (var field in fields)
        {
            // Add quotes if field contains comma, quote, or newline
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                formattedFields.Add($"\"{field.Replace("\"", "\"\"")}\"");
            }
            else
            {
                formattedFields.Add(field);
            }
        }

        return string.Join(",", formattedFields);
    }

    // Helper method to properly parse CSV lines with quoted fields
    private static string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        var currentField = new System.Text.StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(currentField.ToString());
                currentField.Clear();
            }
            else
            {
                currentField.Append(c);
            }
        }

        // Add the last field
        result.Add(currentField.ToString());

        return result.ToArray();
    }

    public void RemoveQuestion()
    {
        if (!isLoggedIn)
        {
            Console.WriteLine("You must be logged in to perform this action.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Question ID to remove: ");
        if (!int.TryParse(Console.ReadLine(), out int questionId))
        {
            Console.WriteLine("Invalid Question ID. Please enter a number.");
            return;
        }

        if (!File.Exists(questionFilePath))
        {
            Console.WriteLine("No questions found.");
            return;
        }

        // Read all lines from the file
        var lines = File.ReadAllLines(questionFilePath);
        var updatedLines = new List<string>();
        bool found = false;
        bool isHeader = true;

        // Keep all lines except the one to remove
        foreach (var line in lines)
        {
            // Always keep the header line
            if (isHeader)
            {
                updatedLines.Add(line);
                isHeader = false;
                continue;
            }

            // Parse the line to get the question ID
            var values = ParseCsvLine(line);
            if (values.Length > 0 && int.TryParse(values[0], out int lineQuestionId))
            {
                if (lineQuestionId == questionId && !found)
                {
                    found = true; // Remove only the first match
                    continue;
                }
            }

            updatedLines.Add(line);
        }

        if (found)
        {
            // Write the updated list back to the file
            File.WriteAllLines(questionFilePath, updatedLines);
            Console.WriteLine($"Question with ID '{questionId}' removed successfully.");
        }
        else
        {
            Console.WriteLine($"Question with ID '{questionId}' not found.");
        }
    }

    public void EditQuestion()
    {
        if (!isLoggedIn)
        {
            Console.WriteLine("You must be logged in to perform this action.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Question ID to edit: ");
        if (!int.TryParse(Console.ReadLine(), out int questionId))
        {
            Console.WriteLine("Invalid Question ID. Please enter a number.");
            return;
        }

        if (!File.Exists(questionFilePath))
        {
            Console.WriteLine("No questions found.");
            return;
        }

        // Read all lines from the file
        var lines = File.ReadAllLines(questionFilePath);
        var updatedLines = new List<string>();
        bool found = false;
        bool isHeader = true;

        // Find and update the question
        foreach (var line in lines)
        {
            // Always keep the header line
            if (isHeader)
            {
                updatedLines.Add(line);
                isHeader = false;
                continue;
            }

            // Parse the line to get the question ID
            var values = ParseCsvLine(line);
            if (values.Length >= 5 && int.TryParse(values[0], out int lineQuestionId))
            {
                if (lineQuestionId == questionId && !found)
                {
                    found = true;
                    
                    // Display current question details
                    Console.WriteLine("\nCurrent Question Details:");
                    Console.WriteLine($"ID: {values[0]}");
                    Console.WriteLine($"Text: {values[1]}");
                    Console.WriteLine($"Options: {values[2]}");
                    Console.WriteLine($"Correct Answer: {values[3]}");
                    Console.WriteLine($"Difficulty: {values[4]}");
                    Console.WriteLine();

                    // Get new values (press Enter to keep current value)
                    Console.Write($"Enter new question text (or press Enter to keep current): ");
                    string newText = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newText))
                        newText = values[1];

                    Console.Write($"Enter new question options (or press Enter to keep current): ");
                    string newOptions = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newOptions))
                        newOptions = values[2];

                    Console.Write($"Enter new correct answer (or press Enter to keep current): ");
                    string newAnswer = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newAnswer))
                        newAnswer = values[3];

                    Console.Write($"Enter new difficulty level (or press Enter to keep current): ");
                    string newDifficulty = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newDifficulty))
                        newDifficulty = values[4];

                    // Add the updated line with proper CSV formatting
                    updatedLines.Add(FormatCsvLine(questionId.ToString(), newText, newOptions, newAnswer, newDifficulty));
                    continue;
                }
            }

            updatedLines.Add(line);
        }

        if (found)
        {
            // Write the updated list back to the file
            File.WriteAllLines(questionFilePath, updatedLines);
            Console.WriteLine($"\nQuestion with ID '{questionId}' updated successfully.");
        }
        else
        {
            Console.WriteLine($"Question with ID '{questionId}' not found.");
        }
    }

    public void ShowAllQuestions()
    {
        if (!isLoggedIn)
        {
            Console.WriteLine("You must be logged in to perform this action.");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        if (!File.Exists(questionFilePath))
        {
            Console.WriteLine("No questions found.");
            return;
        }

        var lines = File.ReadAllLines(questionFilePath);
        bool isHeader = true;
        
        Console.WriteLine("\n=== All Questions ===\n");
        
        foreach (var line in lines)
        {
            // Skip header line
            if (isHeader)
            {
                isHeader = false;
                continue;
            }

            var values = ParseCsvLine(line);
            if (values.Length >= 5)
            {
                Console.WriteLine($"ID: {values[0]}");
                Console.WriteLine($"Text: {values[1]}");
                Console.WriteLine($"Options: {values[2]}");
                Console.WriteLine($"Correct Answer: {values[3]}");
                Console.WriteLine($"Difficulty: {values[4]}");
                Console.WriteLine();
            }
        }
    }
}

