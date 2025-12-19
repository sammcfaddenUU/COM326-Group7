using System;

public class Student : User
{
    
    

        private string status;
    public string Status
    {
        get { return status; }
        set { status = value; }
    }
    // Constructor
    public Student(string username, string password, string status) : base(username, password, "Student")
    {
        this.status = status;
    }
}
