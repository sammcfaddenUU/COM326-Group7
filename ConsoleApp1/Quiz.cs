using System;
using System.Collections.Generic;

public class Quiz
{
    private List<Question> quizQuestions;
    private int quizID;
    private string quizTitle;
    private string quizDescription;
    private DateTime quizDate;

    public int QuizID
    {
        get { return quizID; }
        set { quizID = value; }
    }

    public string QuizTitle
    {
        get { return quizTitle; }
        set { quizTitle = value; }
    }

    public string QuizDescription
    {
        get { return quizDescription; }
        set { quizDescription = value; }
    }

    public DateTime QuizDate
    {
        get { return quizDate; }
        set { quizDate = value; }
    }

    // ---------------- CONSTRUCTORS ----------------

    // Loads all questions
    public Quiz()
    {
        quizQuestions = Question.GetAllQuestions();
    }

    // Parameterized constructor
    public Quiz(int quizID, string quizTitle, string quizDescription, DateTime quizDate)
    {
        this.QuizID = quizID;
        this.QuizTitle = quizTitle;
        this.QuizDescription = quizDescription;
        this.QuizDate = quizDate;

        quizQuestions = Question.GetAllQuestions();
    }

    // ---------------- REQUIRED SAMPLE QUIZZES ----------------
    // EXACTLY matches the assignment table

    public static List<Quiz> LoadSampleQuizzes()
    {
        List<Quiz> quizzes = new List<Quiz>();

        quizzes.Add(new Quiz(
            1,
            "OOP Fundamentals",
            "Covers basics of object-oriented programming",
            new DateTime(2025, 9, 1)
        ));

        quizzes.Add(new Quiz(
            2,
            "Data Structures",
            "Focuses on arrays, lists, stacks, queues, trees, and their applications.",
            new DateTime(2025, 9, 1)
        ));

        quizzes.Add(new Quiz(
            3,
            "Software Design",
            "Includes design patterns, architecture principles, and system modelling.",
            new DateTime(2025, 9, 1)
        ));

        quizzes.Add(new Quiz(
            4,
            "Web Development",
            "HTML, CSS, JavaScript, and client-server interactions",
            new DateTime(2025, 9, 7)
        ));

        quizzes.Add(new Quiz(
            5,
            "Database Systems",
            "SQL queries, relational models, normalization, and transactions.",
            new DateTime(2025, 9, 7)
        ));

        quizzes.Add(new Quiz(
            6,
            "Cybersecurity Basics",
            "Encryption, authentication, and common security threats",
            new DateTime(2025, 9, 11)
        ));

        quizzes.Add(new Quiz(
            7,
            "Computer Networks",
            "Protocols, IP addressing, routing, and network layers",
            new DateTime(2025, 9, 13)
        ));

        return quizzes;
    }
}

