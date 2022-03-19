using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Services.Utilities
{
    public static class Messages
    {
        public static class Category{
            public static string NotFound(bool isPlural){
                if(isPlural) return "No categories found.";
                return "Category not found.";
            }
            public static string Get(bool isPlural, string categoryName = null){
                if(isPlural) return "Categories has successfully been brought.";
                else return $"{categoryName} has successfully been brought.";
            }
            public static string Add(string categoryName){
                return $"{categoryName} has successfully been added.";
            }
            public static string Update(string categoryName){
                return $"{categoryName} has successfully been updated.";
            }
            public static string Delete(string categoryName){
                return $"{categoryName} has succssfully been deleted.";
            }
            public static string HardDelete(string categoryName){
                return $"{categoryName} has successfully been deleted from database.";
            }
            public static string Count(int categoryCount){
                return $"{categoryCount} categories has successfully been brought.";
            }
        }

        public static class Article{
            public static string NotFound(bool isPlural){
                if(isPlural) return "No articles found.";
                return "Article not found.";
            }
            public static string Get(bool isPlural, string articleName = null){
                if(isPlural) return "Articles has successfully been brought.";
                else return $"{articleName} has successfully been brought.";
            }
            public static string Add(string articleName){
                return $"{articleName} has successfully been added.";
            }
            public static string Update(string articleName){
                return $"{articleName} has successfully been updated.";
            }
            public static string Delete(string articleName){
                return $"{articleName} has succssfully been deleted.";
            }
            public static string HardDelete(string articleName){
                return $"{articleName} has successfully been deleted from database.";
            }
            public static string Count(int articleCount){
                return $"{articleCount} articles has successfully been brought.";
            }
        }

        public static class Comment{
            public static string NotFound(bool isPlural){
                if(isPlural) return "No comments found.";
                return "Comment not found.";
            }
            public static string Get(bool isPlural, string commentName = null){
                if(isPlural) return "Comments has successfully been brought.";
                else return $"{commentName} has successfully been brought.";
            }
            public static string Add(string commentName){
                return $"{commentName} has successfully been added.";
            }
            public static string Update(string commentName){
                return $"{commentName} has successfully been updated.";
            }
            public static string Delete(string commentName){
                return $"{commentName} has succssfully been deleted.";
            }
            public static string HardDelete(string commentName){
                return $"{commentName} has successfully been deleted from database.";
            }
            public static string Count(int commentCount){
                return $"{commentCount} comments has successfully been brought.";
            }
        }
    }
}