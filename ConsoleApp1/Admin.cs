using System;
using System.Collections.Generic;
using System.IO;

public class Admin : User
{
    private DateTime loginDate;
    private const string questionFilePath = "Question.csv";

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
    public void AddquizCatagory()
    {
        Console.Write("Enter quiz category name: ");
        string categoryName = Console.ReadLine();
        // Here you would typically add the category to a database or a list
        Console.WriteLine($"Quiz category '{categoryName}' added.");

        using (var writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"{categoryName}");
        }
    }

    public void RemoveQuizCategory()
    {
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
        Console.Write("Enter Question ID: ");
        if (!int.TryParse(Console.ReadLine(), out int questionId))
        {
            Console.WriteLine("Invalid Question ID. Please enter a number.");
            return;
        }

        Console.Write("Enter question text: ");
        string questionText = Console.ReadLine();

        Console.Write("Enter question options (comma-separated): ");
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

        // Append the new question to the CSV file
        using (var writer = new StreamWriter(questionFilePath, true))
        {
            writer.WriteLine($"{questionId},{questionText},{questionOptions},{correctAnswer},{difficulty}");
        }

        Console.WriteLine($"Question with ID '{questionId}' added successfully.");
    }

    public void RemoveQuestion()
    {
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
            var values = line.Split(',');
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
            var values = line.Split(',');
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
                    Console.Write($"Enter new question text (or press Enter to keep '{values[1]}'): ");
                    string newText = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newText))
                        newText = values[1];

                    Console.Write($"Enter new question options (or press Enter to keep '{values[2]}'): ");
                    string newOptions = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newOptions))
                        newOptions = values[2];

                    Console.Write($"Enter new correct answer (or press Enter to keep '{values[3]}'): ");
                    string newAnswer = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newAnswer))
                        newAnswer = values[3];

                    Console.Write($"Enter new difficulty level (or press Enter to keep '{values[4]}'): ");
                    string newDifficulty = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newDifficulty))
                        newDifficulty = values[4];

                    // Add the updated line
                    updatedLines.Add($"{questionId},{newText},{newOptions},{newAnswer},{newDifficulty}");
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
        if (!File.Exists(questionFilePath))
        {
            Console.WriteLine("No questions found.");
            return;
        }
        var lines = File.ReadAllLines(questionFilePath);
        bool isHeader = true;
        foreach (var line in lines)
        {
            // Skip header line
            if (isHeader)
            {
                isHeader = false;
                continue;
            }
            var values = line.Split(',');
            if (values.Length >= 5)
            {
                Console.WriteLine($"ID: {values[0]}, Text: {values[1]}, Options: {values[2]}, Correct Answer: {values[3]}, Difficulty: {values[4]}");
            }
        }
    }
}