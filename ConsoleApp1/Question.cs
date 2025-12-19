using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


public class Question
{
    // Private fields store question data
    private int questionID;
    private string questionText;
    private string questionCorrectAnswer;
    private string questionDifficulty;
    private string questionOptions;

    // Static list holding all questions from CSV file
    // Shared accross the application
    private static List<Question> QuestionLists = new List<Question>();

    public static List<Question> GetAllQuestions()
    {
        return QuestionLists;
    }

    // Loads questions from the CSV file into the static list
    // Called once at application startup
    public static void LoadQuestion(string destinationFilePath)
    {
        using (var reader = new StreamReader(destinationFilePath))
        {
            // Skip the heqader line
            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                // Read each value into the relevant variable
                int QuestionID = int.Parse(values[0]);
                string QuestionText = values[1];
                string QuestionOptions = values[2];
                string CorrectAnswer = values[3];
                string DifficultyLevel = values[4];

                // Create a question and add it to the list
                Question q = new Question(QuestionID, QuestionText, QuestionOptions, CorrectAnswer, DifficultyLevel);
                QuestionLists.Add(q);
            }
        }
    }

    // Used when adding a new question
    public static void SaveQuestionToCSV(string filePath, Question q)

    {
        using (var writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"{q.QuestionID},{q.QuestionText},{q.QuestionOptions},{q.QuestionCorrectAnswer},{q.QuestionDifficulty}");
        }
    }

    // Rewrites the entire CSV file with the updated list of questions
    // Used after editing or removing a question
    public static void SaveAllQuestionsToCSV(string filePath)
    {
        using (var writer = new StreamWriter(filePath, false))
        {
            // Write header
            writer.WriteLine("QuestionID,QuestionText,QuestionOptions,CorrectAnswer,QuestionDifficulty");

            // Write each question to the file
            foreach (Question q in QuestionLists)
            {
                writer.WriteLine($"{q.QuestionID},{q.QuestionText},{q.QuestionOptions},{q.QuestionCorrectAnswer},{q.QuestionDifficulty}");
            }
        }
    }

    // Updates the fields of the question
    // Called when editing a question
    public void Update(string newText, string newOptions, string newAnswer, string newDifficulty)
    {
        QuestionText = newText;
        QuestionOptions = newOptions;
        QuestionCorrectAnswer = newAnswer;
        QuestionDifficulty = newDifficulty;
    }

    // Public properties for accessing question data
    public int QuestionID
    {
        get { return questionID; }
        set { questionID = value; }
    }

    public string QuestionText
    {
        get { return questionText; }
        set { questionText = value; }
    }

    public string QuestionCorrectAnswer
    {
        get { return questionCorrectAnswer; }
        set { questionCorrectAnswer = value; }
    }

    public string QuestionDifficulty
    {
        get { return questionDifficulty; }
        set { questionDifficulty = value; }
    }

    public string QuestionOptions
    {
        get { return questionOptions; }
        set { questionOptions = value; }
    }

    // Constructor
    public Question(int questionID, string questionText, string questionOptions, string questionCorrectAnswer, string questionDifficulty)
    {
        this.questionID = questionID;
        this.questionText = questionText;
        this.questionCorrectAnswer = questionCorrectAnswer;
        this.questionDifficulty = questionDifficulty;
        this.questionOptions = questionOptions;
    }

}
