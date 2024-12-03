using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD
namespace TechCareer.Service.Constants;

public static class CategoryMessages
{
    public const string CategoryNotFound = "Category not found.";
    public const string CategoryTitleAlreadyExists = "Category title already exists.";
    public const string CategoryDeleted = "Category successfully deleted.";

    
}
=======
namespace TechCareer.Service.Constants
{
    public static class CategoryMessages
    {
        public const string CategoryNotFound = "Category not found.";
        public const string CategoryTitleAlreadyExists = "Category title already exists.";
        public const string CategoryDeleted = "Category successfully deleted.";

        public static string? CategoryNameAlreadyExists { get; internal set; }
    }
}
>>>>>>> 716439d72e0a58805e01a0ae2e996290e82d92e6
