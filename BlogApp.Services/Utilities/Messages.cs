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
            public static string UndoDelete(string categoryName){
                return $"{categoryName} has successfully been brought back.";
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
            public static string UndoDelete(string articleName){
                return $"{articleName} has successfully been brought back.";
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
            public static string Get(bool isPlural, int commentId = -1){
                if(isPlural) return "Comments has successfully been brought.";
                else return $"Comment it's id {commentId} has successfully been brought.";
            }
            public static string Add(string commentText){
                if(commentText.Length > 50) return $"{commentText.Substring(0, 50) + "..."} has successfully been added.";
                return $"{commentText} has successfully been added.";
            }
            public static string Update(string commentText){
                if(commentText.Length > 50) return $"{commentText.Substring(0, 50) + "..."} has successfully been updated.";
                return $"{commentText} has successfully been updated.";
            }
            public static string Delete(string commentText){
                if(commentText.Length > 50) return $"{commentText.Substring(0, 50) + "..."} has successfully been deleted.";
                return $"{commentText} has successfully been deleted.";
            }
            public static string HardDelete(string commentText){
                if(commentText.Length > 50) return $"{commentText.Substring(0, 50) + "..."} has successfully been deleted from database.";
                return $"{commentText} has successfully been added.";
            }
            public static string UndoDelete(string commentText){
                if(commentText.Length > 50) return $"{commentText.Substring(0, 50) + "..."} has successfully been brought back.";
                return $"{commentText} has successfully been brought back.";
            }
            public static string Approve(string commentText){
                if(commentText.Length > 50) return $"{commentText.Substring(0, 50) + "..."} has successfully been approved.";
                return $"{commentText} has successfully been approved.";
            }
            public static string Count(int commentCount){
                return $"{commentCount} comments has successfully been brought.";
            }
        }
    }
}