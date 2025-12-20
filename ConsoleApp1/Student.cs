// Author Samuel McFadden

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Student : User
{
    private string status;
    private const string resultsFilePath = "quiz_results.csv";
    private List<QuizResult> quizHistory;

    public string Status
    {
        get { return status; }
        set { status = value; }
    }

    // Constructor
    public Student(string username, string password, string status) : base(username, password, "Student")
    {
        this.status = status;
        quizHistory = new List<QuizResult>();
        LoadQuizHistory();
    }

    public void PlayQuiz()
    {
        Console.Clear();
        Console.WriteLine("=== Quiz Application ===\n");

        List<Question> availableQuestions = Question.GetAllQuestions();

        if (availableQuestions == null || availableQuestions.Count == 0)
        {
            Console.WriteLine("No questions available. Please contact an administrator.");
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
            return;
        }

        // Select Category
        string selectedCategory = SelectCategory();
        if (string.IsNullOrEmpty(selectedCategory))
        {
            return;
        }

        List<Question> quizQuestions = availableQuestions;

        if (quizQuestions.Count == 0)
        {
            Console.WriteLine("\nNo questions available.");
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
            return;
        }

        // Select number of questions
        Console.Write($"\nHow many questions would you like to answer? (Max: {quizQuestions.Count}): ");
        if (int.TryParse(Console.ReadLine(), out int numQuestions) && numQuestions > 0)
        {
            numQuestions = Math.Min(numQuestions, quizQuestions.Count);
            quizQuestions = quizQuestions.Take(numQuestions).ToList();
        }

        // Start the quiz
        int score = 0;
        int questionNumber = 1;

        Console.Clear();
        Console.WriteLine("=== Starting Quiz ===");
        Console.WriteLine($"Category: {selectedCategory}");
        Console.WriteLine($"Questions: {quizQuestions.Count}\n");
        Console.WriteLine("Press any key to begin...");
        Console.ReadKey();

        foreach (Question question in quizQuestions)
        {
            Console.Clear();
            Console.WriteLine($"Question {questionNumber} of {quizQuestions.Count}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"\n{question.QuestionText}\n");

            string[] options = question.QuestionOptions.Split('|');
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i].Trim()}");
            }

            Console.Write("\nYour answer (enter number): ");
            string answerInput = Console.ReadLine();

            if (int.TryParse(answerInput, out int answerIndex) && answerIndex > 0 && answerIndex <= options.Length)
            {
                string studentAnswer = options[answerIndex - 1].Trim();

                if (studentAnswer.Equals(question.QuestionCorrectAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\nCorrect!");
                    score++;
                }
                else
                {
                    Console.WriteLine("\nIncorrect!");
                    Console.WriteLine($"The correct answer was: {question.QuestionCorrectAnswer}");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input - marked as incorrect.");
                Console.WriteLine($"The correct answer was: {question.QuestionCorrectAnswer}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            questionNumber++;
        }

        // Save quiz result
        QuizResult result = new QuizResult(Username, selectedCategory, "All", quizQuestions.Count, score);
        quizHistory.Add(result);
        SaveQuizResult(result);

        // Display results
        DisplayQuizResults(result);
    }

    private string SelectCategory()
    {
        Console.WriteLine("\n=== Select Quiz Category ===");

        if (!File.Exists(filePath))
        {
            Console.WriteLine("No categories available. Using 'General' as default.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return "General";
        }

        var categories = File.ReadAllLines(filePath).Where(c => !string.IsNullOrWhiteSpace(c)).ToList();

        if (categories.Count == 0)
        {
            Console.WriteLine("No categories available. Using 'General' as default.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return "General";
        }

        Console.WriteLine("\nAvailable Categories:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i]}");
        }
        Console.WriteLine($"{categories.Count + 1}. Cancel");

        Console.Write("\nSelect a category: ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= categories.Count)
        {
            return categories[choice - 1];
        }
        else if (choice == categories.Count + 1)
        {
            return null;
        }

        Console.WriteLine("Invalid selection. Using first category.");
        return categories[0];
    }

    private void DisplayQuizResults(QuizResult result)
    {
        Console.Clear();
        Console.WriteLine("=== Quiz Results ===\n");

        Console.WriteLine($"Student: {result.StudentUsername}");
        Console.WriteLine($"Category: {result.Category}");
        Console.WriteLine($"Difficulty: {result.Difficulty}");
        Console.WriteLine($"Date: {result.DateTaken:yyyy-MM-dd HH:mm}");
        Console.WriteLine();
        Console.WriteLine($"Questions Answered: {result.TotalQuestions}");
        Console.WriteLine($"Correct Answers: {result.CorrectAnswers}");
        Console.WriteLine($"Incorrect Answers: {result.TotalQuestions - result.CorrectAnswers}");
        Console.WriteLine($"Score: {result.Percentage:F2}%");
        Console.WriteLine($"Grade: {result.GetGrade()}");

        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
    }

    public void ViewFeedback()
    {
        Console.Clear();
        Console.WriteLine("=== Overall Performance Feedback ===\n");

        if (quizHistory.Count == 0)
        {
            Console.WriteLine("No quiz history found. Complete some quizzes to see your feedback!");
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
            return;
        }

        // Calculate overall statistics
        int totalQuizzes = quizHistory.Count;
        int totalQuestions = quizHistory.Sum(r => r.TotalQuestions);
        int totalCorrect = quizHistory.Sum(r => r.CorrectAnswers);
        double overallPercentage = (double)totalCorrect / totalQuestions * 100;

        Console.WriteLine($"Student: {Username}");
        Console.WriteLine($"Total Quizzes Taken: {totalQuizzes}");
        Console.WriteLine($"Total Questions Answered: {totalQuestions}");
        Console.WriteLine($"Total Correct Answers: {totalCorrect}");
        Console.WriteLine($"Overall Accuracy: {overallPercentage:F2}%");
        Console.WriteLine();

        // Simple performance feedback
        string feedback = overallPercentage switch
        {
            >= 90 => "Outstanding performance! Keep up the excellent work!",
            >= 80 => "Very good performance! You're doing well.",
            >= 70 => "Good performance! You have a solid understanding.",
            >= 60 => "Satisfactory performance. Keep practicing to improve.",
            _ => "Needs improvement. Consider reviewing the material more."
        };

        Console.WriteLine(feedback);

        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
    }

    private void LoadQuizHistory()
    {
        if (!File.Exists(resultsFilePath))
        {
            return;
        }

        try
        {
            var lines = File.ReadAllLines(resultsFilePath);
            bool isHeader = true;

            foreach (var line in lines)
            {
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                var values = line.Split(',');
                if (values.Length >= 6 && values[0] == Username)
                {
                    var result = new QuizResult(
                        values[0],
                        values[1],
                        values[2],
                        int.Parse(values[3]),
                        int.Parse(values[4])
                    );
                    
                    if (DateTime.TryParse(values[5], out DateTime date))
                    {
                        result.DateTaken = date;
                    }

                    quizHistory.Add(result);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading quiz history: {ex.Message}");
        }
    }

    private void SaveQuizResult(QuizResult result)
    {
        try
        {
            if (!File.Exists(resultsFilePath))
            {
                using (var writer = new StreamWriter(resultsFilePath, false))
                {
                    writer.WriteLine("Username,Category,Difficulty,TotalQuestions,CorrectAnswers,DateTaken,Percentage");
                }
            }

            using (var writer = new StreamWriter(resultsFilePath, true))
            {
                writer.WriteLine($"{result.StudentUsername},{result.Category},{result.Difficulty},{result.TotalQuestions},{result.CorrectAnswers},{result.DateTaken:yyyy-MM-dd HH:mm:ss},{result.Percentage:F2}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving quiz result: {ex.Message}");
        }
    }
}

