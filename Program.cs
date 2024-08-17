using categoryTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter Category Id");
        int userChoice = int.Parse(Console.ReadLine());

        var category = GetCategoryDetails(userChoice);

        if (category != null)
        {
            Console.WriteLine($"Input: {userChoice}; Output: ParentCategoryID = {category.ParentCategoryId}, Name = {category.Name}, Keywords = {category.Keywords} \n");
        }
        else
        {
            Console.WriteLine("No Category Found!");
        }

        Console.WriteLine("Enter Category level.");
        userChoice = int.Parse(Console.ReadLine());

        var categoryLevel = GetCategoryLevel(userChoice);

        Console.WriteLine($"Input: {userChoice}; Output: {(categoryLevel.Count == 0 ? "Not found" : string.Join(", ", categoryLevel))} \n");
    }

    static Category? GetCategoryDetails(int categoryId)
    {
        var tempCategory = GetCategories().FirstOrDefault(x => x.CategoryId == categoryId);
        if (tempCategory == null)
            return null;

        var category = tempCategory;

        while (string.IsNullOrEmpty(tempCategory!.Keywords) && tempCategory.ParentCategoryId != -1)
        {
            tempCategory = GetCategories().FirstOrDefault(x => x.CategoryId == tempCategory.ParentCategoryId);
        }

        if(string.IsNullOrEmpty(category!.Keywords))
        {
            category.Keywords = tempCategory.Keywords;
        }

        return category;
    }

    static List<int> GetCategoryLevel(int targetLevel)
    {
        return GetCategories()
            .Where(c => CalculateCategoryLevel(c.CategoryId) == targetLevel)
            .Select(c => c.CategoryId)
            .ToList();
    }

    static int CalculateCategoryLevel(int categoryId)
    {
        int level = 0;
        var category = GetCategories().FirstOrDefault(c => c.CategoryId == categoryId);

        while (category != null && category.ParentCategoryId != -1)
        {
            level++;
            category = GetCategories().FirstOrDefault(c => c.CategoryId == category.ParentCategoryId);
        }

        return level;
    }

    static List<Category> GetCategories()
    {
        return new List<Category>
        {
            new() { CategoryId = 100, ParentCategoryId = -1, Name = "Business", Keywords = "Money" },
            new() { CategoryId = 200, ParentCategoryId = -1, Name = "Tutoring", Keywords = "Teaching" },
            new() { CategoryId = 101, ParentCategoryId = 100, Name = "Accounting", Keywords = "Taxes" },
            new() { CategoryId = 102, ParentCategoryId = 100, Name = "Taxation", Keywords = null },
            new() { CategoryId = 201, ParentCategoryId = 200, Name = "Computer", Keywords = null },
            new() { CategoryId = 103, ParentCategoryId = 101, Name = "Corporate Tax", Keywords = null },
            new() { CategoryId = 202, ParentCategoryId = 201, Name = "Operating System", Keywords = null },
            new() { CategoryId = 109, ParentCategoryId = 101, Name = "Small business Tax", Keywords = null },
            new() { CategoryId = 110, ParentCategoryId = 109, Name = "License Tax", Keywords = null }
        };
    }
}
