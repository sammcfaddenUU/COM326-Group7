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

    // Load categories from a text file
    public static List<Category> LoadCategories(string filePath)
    {
        List<Category> categories = new List<Category>();
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i+=3)
        {
            int id = int.Parse(lines[i]);
            string name = lines[i + 1];
            string description = lines[i + 2];

            Category category = new Category(id, name, description);
            categories.Add(category);
        }

        return categories;
    }
    // Filter categories by name
    public static Category Filter(List<Category> categories, int id)
    {
        foreach (Category category in categories)
        {
            if (category.CategoryName == id.ToString())
            {
                return category;
            }
        }
        return null;
    }
}
