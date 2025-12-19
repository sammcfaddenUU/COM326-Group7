using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class QuizSystem
{
    // File path where CSV is stored
    private static string destinationFilePath = string.Empty;

    // Loads questions from CSV and starts the menu
    public static void Main()
    {
        try
        {
            string folder = "Data";
            string filename = "Question.csv";

            // Copy CSV into working directory
            string filePath = CopyDataToWorkingDir(folder, filename);
            destinationFilePath = filePath;

            Console.WriteLine($"Loading questions from: {filePath}");

            // Load all questions into memory
            Question.LoadQuestion(filePath);

            Console.WriteLine($"Loaded {Question.GetAllQuestions().Count} questions successfully.");

            // Start the main menu
            MainMenu();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    // Copies the CSV file from the source folder to the working directory
    private static string CopyDataToWorkingDir(string folder, string filename)
    {
        // Get the application's base directory
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        
        Console.WriteLine($"Base directory: {baseDirectory}");
        
        // Build the source path (where the file is stored in your project)
        string sourcePath = Path.Combine(baseDirectory, folder, filename);
        
        Console.WriteLine($"Looking for file at: {sourcePath}");
        
        // Build the destination path (where we want to copy it for working)
        string destinationPath = Path.Combine(baseDirectory, filename);

        // Check if source file exists
        if (!File.Exists(sourcePath))
        {
            throw new FileNotFoundException($"Could not find the file: {sourcePath}\n\nMake sure the Data folder exists in your output directory (bin\\Debug\\net8.0\\Data)");
        }

        Console.WriteLine("Copying file to working directory...");
        
        // Copy the file to the working directory
        File.Copy(sourcePath, destinationPath, overwrite: true);
        
        return destinationPath;
    }

    // Main menu
    public static void MainMenu()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. Edit Question");
            Console.WriteLine("2. Show Correct Answer for Chosen Question");
            Console.WriteLine("3. Show All Questions");
            Console.WriteLine("4. Add Question");
            Console.WriteLine("5. Remove Question");
            Console.WriteLine("6. Exit");

            Console.Write("Select an option: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    EditQuestion();
                    break;
                case "2":
                    ShowAnswer();
                    break;
                case "3":
                    ShowAllQuestions();
                    break;
                case "4":
                    AddQuestion();
                    break;
                case "5":
                    RemoveQuestion();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    // Edit a question
    // Updates object and rewrites the CSV file
    public static void EditQuestion()
    {
        Console.WriteLine("Enter Question ID to edit:");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        List<Question> questions = Question.GetAllQuestions();
        Question? q = questions.FirstOrDefault(x => x.QuestionID == id);

        if (q == null)
        {
            Console.WriteLine("Question not found.");
            return;
        }

        Console.WriteLine("Enter new question text:");
        string? newText = Console.ReadLine();

        Console.WriteLine("Enter new options:");
        string? newOptions = Console.ReadLine();

        Console.WriteLine("Enter new correct answer:");
        string? newAnswer = Console.ReadLine();

        Console.WriteLine("Enter new difficulty:");
        string? newDifficulty = Console.ReadLine();

        q.Update(newText ?? "", newOptions ?? "", newAnswer ?? "", newDifficulty ?? "");
        Question.SaveAllQuestionsToCSV(destinationFilePath);

        Console.WriteLine("Question updated successfully.");
    }

    // Display the correct answer for a question
    public static void ShowAnswer()
    {
        Console.WriteLine("Enter Question ID:");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        Question? q = Question.GetAllQuestions().FirstOrDefault(x => x.QuestionID == id);

        if (q == null)
        {
            Console.WriteLine("Question not found.");
            return;
        }

        Console.WriteLine($"Correct Answer: {q.QuestionCorrectAnswer}");
    }

    // Show all questions
    public static void ShowAllQuestions()
    {
        List<Question> questions = Question.GetAllQuestions();
        
        if (questions.Count == 0)
        {
            Console.WriteLine("No questions available.");
            return;
        }

        foreach (Question q in questions)
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"ID: {q.QuestionID}");
            Console.WriteLine($"Text: {q.QuestionText}");
            Console.WriteLine($"Options: {q.QuestionOptions}");
            Console.WriteLine($"Correct Answer: {q.QuestionCorrectAnswer}");
            Console.WriteLine($"Difficulty: {q.QuestionDifficulty}");
        }
        Console.WriteLine(new string('-', 40));
    }

    // Add a new question
    public static void AddQuestion()
    {
        Console.WriteLine("Enter Question ID:");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        Console.WriteLine("Enter Question Text:");
        string? text = Console.ReadLine();

        Console.WriteLine("Enter Question Options:");
        string? options = Console.ReadLine();

        Console.WriteLine("Enter Correct Answer:");
        string? answer = Console.ReadLine();

        Console.WriteLine("Enter Difficulty Level:");
        string? difficulty = Console.ReadLine();

        Question newQ = new Question(id, text ?? "", options ?? "", answer ?? "", difficulty ?? "");
        Question.GetAllQuestions().Add(newQ);
        Question.SaveQuestionToCSV(destinationFilePath, newQ);
        Console.WriteLine("Question added successfully.");
    }

    // Remove a question by ID
    // Updates the list and rewrites the CSV file
    public static void RemoveQuestion()
    {
        Console.WriteLine("Enter Question ID to remove:");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        List<Question> questions = Question.GetAllQuestions();
        Question? q = questions.FirstOrDefault(x => x.QuestionID == id);

        if (q == null)
        {
            Console.WriteLine("Question not found.");
            return;
        }

        questions.Remove(q);
        Question.SaveAllQuestionsToCSV(destinationFilePath);
        Console.WriteLine("Question removed successfully.");
    }
}