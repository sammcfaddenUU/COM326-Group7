using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    // Loads all questions
    public Quiz()
    {
        quizQuestions = Question.GetAllQuestions();
    }
    // Constructor
    public Quiz(int quizID, string quizTitle, string quizDescription, DateTime quizDate)
    {
        this.QuizID = quizID;
        this.QuizTitle = quizTitle;
        this.QuizDescription = quizDescription;
        this.QuizDate = quizDate;

        quizQuestions = Question.GetAllQuestions();
    }
}


