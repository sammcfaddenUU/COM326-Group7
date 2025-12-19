using System;

public class Category
{
    
    

        private int categoryID;
    private string categoryName;
    private string categoryDescription;

    public int CategoryID
    {
        get { return categoryID; }
        set { categoryID = value; }

    }
    public string CategoryName
    {
        get { return categoryName; }
        set { categoryName = value; }
    }
    public string CategoryDescription
    {
        get { return categoryDescription; }
        set { categoryDescription = value; }
    }
    // Constructor
    public Category(int categoryID, string categoryName, string categoryDescription)
    {
        this.categoryID = categoryID;
        this.categoryName = categoryName;
        this.categoryDescription = categoryDescription;
    }
}
