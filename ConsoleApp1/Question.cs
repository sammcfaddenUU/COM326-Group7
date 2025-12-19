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
    // Shared across the application
    private static List<Question> QuestionLists = new List<Question>();

    public static List<Question> GetAllQuestions()
    {
        return QuestionLists;
    }

    // Loads questions from the CSV file into the static list
    // Called once at application startup
    public static void LoadQuestion(string destinationFilePath)
    {
        QuestionLists.Clear(); // Clear existing questions
        
        using (var reader = new StreamReader(destinationFilePath))
        {
            // Skip the header line
            reader.ReadLine();
            
            int lineNumber = 1; // Track line number for error reporting
            
            while (!reader.EndOfStream)
            {
                lineNumber++;
                try
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    
                    // Use proper CSV parsing to handle quotes and commas
                    var values = ParseCsvLine(line);

                    if (values.Count < 5)
                    {
                        Console.WriteLine($"Warning: Skipping line {lineNumber} - insufficient columns ({values.Count} found, 5 expected)");
                        continue;
                    }

                    // Read each value into the relevant variable
                    int QuestionID = int.Parse(values[0].Trim());
                    string QuestionText = values[1].Trim();
                    string QuestionOptions = values[2].Trim();
                    string CorrectAnswer = values[3].Trim();
                    string DifficultyLevel = values[4].Trim();

                    // Create a question and add it to the list
                    Question q = new Question(QuestionID, QuestionText, QuestionOptions, CorrectAnswer, DifficultyLevel);
                    QuestionLists.Add(q);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing line {lineNumber}: {ex.Message}");
                    Console.WriteLine($"Line content: {reader.ReadLine()}");
                }
            }
        }
    }

    // Parses a CSV line properly handling quotes and commas
    private static List<string> ParseCsvLine(string line)
    {
        List<string> fields = new List<string>();
        StringBuilder currentField = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    // Escaped quote (two quotes in a row)
                    currentField.Append('"');
                    i++; // Skip next quote
                }
                else
                {
                    // Toggle quote mode
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                // End of field
                fields.Add(currentField.ToString());
                currentField.Clear();
            }
            else
            {
                currentField.Append(c);
            }
        }

        // Add the last field
        fields.Add(currentField.ToString());

        return fields;
    }

    // Used when adding a new question
    public static void SaveQuestionToCSV(string filePath, Question q)
    {
        using (var writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(FormatCsvLine(q));
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
                writer.WriteLine(FormatCsvLine(q));
            }
        }
    }

    // Formats a question as a CSV line, properly escaping quotes and commas
    private static string FormatCsvLine(Question q)
    {
        return $"{q.QuestionID}," +
               $"\"{EscapeCsvField(q.QuestionText)}\"," +
               $"\"{EscapeCsvField(q.QuestionOptions)}\"," +
               $"\"{EscapeCsvField(q.QuestionCorrectAnswer)}\"," +
               $"\"{EscapeCsvField(q.QuestionDifficulty)}\"";
    }

    // Escapes a CSV field by doubling quotes
    private static string EscapeCsvField(string field)
    {
        if (field == null) return string.Empty;
        return field.Replace("\"", "\"\"");
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